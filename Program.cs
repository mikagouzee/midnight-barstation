var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var wallets = new List<Wallet>
{
    new Wallet
    {
        Owner = "User1",
        CurrentValue = 100.00M,
        LastUpdate = DateTime.UtcNow
    },
    new Wallet
    {
        Owner = "User2",
        CurrentValue = 50.00M,
        LastUpdate = DateTime.UtcNow
    }
};

app.MapPost("/addValue", async (Wallet wallet, decimal amount) =>
{
    if (amount <= 0)
    {
        return Results.BadRequest("Amount must be greater than zero.");
    }
    Console.WriteLine($"Processing wallet {wallet.Owner}");
    // Update wallet balance
    wallet.CurrentValue += amount;

    // Record the transaction
    wallet.LastTransactions.Add(new Transaction
    {
        Timestamp = DateTime.UtcNow,
        Type = "Refill",
        Amount = amount
    });

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

        // Record the transaction
        wallet.LastTransactions.Add(new Transaction
        {
            Timestamp = DateTime.UtcNow,
            Type = "Payment",
            Amount = amount
        });

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

