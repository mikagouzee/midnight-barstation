var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var wallets = new List<Wallet>
{
    new Wallet
    {
        Id = 0,
        Owner = "User1",
        CurrentValue = 100.00M,
        LastUpdate = DateTime.UtcNow
    },
    new Wallet
    {
        Id = 1,
        Owner = "User2",
        CurrentValue = 50.00M,
        LastUpdate = DateTime.UtcNow
    }
};

var transactionHistory = new List<Transaction>();

app.MapPost("/addValue", async (Wallet wallet, decimal amount) =>
{
    if (amount <= 0)
    {
        return Results.BadRequest("Amount must be greater than zero.");
    }
    Console.WriteLine($"Processing wallet {wallet.Owner}");
    // Update wallet balance
    wallet.CurrentValue += amount;

    var current = new Transaction
    {
        Timestamp = DateTime.UtcNow,
        Type = "Refill",
        Amount = amount
    };

    // Record the transaction
    wallet.LastTransactions.Add(current);
    transactionHistory.Add(current);

    // Update the lastUpdate field
    wallet.LastUpdate = DateTime.UtcNow;

    Console.WriteLine($"Refill of {amount:C} processed. New balance: {wallet.CurrentValue:C}");

    return Results.Ok(wallet);
});

app.MapPost("/subtractValue", async (Wallet wallet, decimal amount) =>
{
    if (amount <= 0)
    {
        return Results.BadRequest("Amount must be greater than zero.");
    }
    Console.WriteLine($"Processing wallet {wallet.Owner}");
    if (wallet.CurrentValue >= amount)
    {
        // Update wallet balance
        wallet.CurrentValue -= amount;

        var current = new Transaction
        {
            Timestamp = DateTime.UtcNow,
            Type = "Payment",
            Amount = amount
        };
        // Record the transaction
        wallet.LastTransactions.Add(current);
        transactionHistory.Add(current);

        // Update the lastUpdate field
        wallet.LastUpdate = DateTime.UtcNow;

        Console.WriteLine($"Payment of {amount:C} processed. New balance: {wallet.CurrentValue:C}");

        return Results.Ok(wallet);
    }
    else
    {
        return Results.BadRequest("Insufficient balance.");
    }
});

app.MapGet("/getWallet/{username}", (string username) =>
{
    var wallet = wallets.FirstOrDefault(w => w.Owner == username);
    if (wallet != null)
    {
        return Results.Ok(wallet);
    }
    else
    {
        return Results.NotFound("Wallet not found.");
    }
});

app.Run();

