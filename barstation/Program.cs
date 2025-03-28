// File: Program.cs
using Barstation.Models;
using Barstation.Services;

var builder = WebApplication.CreateBuilder(args);

// Register the WalletOperations service as transient
builder.Services.AddTransient<IWalletOperations, WalletOperations>();
builder.Services.AddTransient<IWalletRepository, WalletRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

app.MapPost("/addValue", (Wallet wallet, decimal amount, IWalletOperations walletOperations, ITransactionRepository transactionRepository) =>
{
    return walletOperations.AddValue(wallet, amount);
});

app.MapPost("/subtractValue", (Wallet wallet, decimal amount, IWalletOperations walletOperations, ITransactionRepository transactionRepository) =>
{
    return walletOperations.SubtractValue(wallet, amount);
});


app.MapPost("/createWallet", (Wallet wallet, IWalletOperations walletOperations) =>
{
    return walletOperations.Create(wallet);
});

app.MapPut("/blockWallet", (string username, IWalletOperations walletOperations) =>
{
    return walletOperations.BlockByOwner(username);
});

app.MapPut("/unlockWallet", (Wallet wallet, IWalletOperations walletOperations ) =>
{
    walletOperations.Unlock(wallet);
});

app.Run();
