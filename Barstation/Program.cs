// File: Program.cs
using Barstation.Models;
using Barstation.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Register the WalletOperations service as transient
builder.Services.AddTransient<IWalletOperations, WalletOperations>();
builder.Services.AddTransient<IWalletRepository, WalletRepository>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();

var app = builder.Build();

app.MapGet("/", () => "Barstation API is running!");

var transactionCounter = Metrics.CreateCounter("transactions_total", "Number of transactions processed");
var errorCounter = Metrics.CreateCounter("errors_total", "Number of errors catched");
var securityCounter = Metrics.CreateCounter("security_operations", "Number of lock/unlock processed");

app.MapPost("/addValue", (Wallet wallet, decimal amount, IWalletOperations walletOperations, ITransactionRepository transactionRepository) =>
{
	try
	{
		var result = walletOperations.AddValue(wallet, amount);
        transactionCounter.Inc();

		return result;
	}
	catch (global::System.Exception)
	{
		errorCounter.Inc();
		throw;
	}
});

app.MapPost("/subtractValue", (Wallet wallet, decimal amount, IWalletOperations walletOperations, ITransactionRepository transactionRepository) =>
{
	try
	{
		var result = walletOperations.SubtractValue(wallet, amount);
		transactionCounter.Inc();
		return result;
	}
	catch (global::System.Exception)
	{
		errorCounter.Inc();
		throw;
	}
});


app.MapPost("/createWallet", (Wallet wallet, IWalletOperations walletOperations) =>
{
	try
	{
		transactionCounter.Inc();
		return walletOperations.Create(wallet);
	}
	catch (global::System.Exception)
	{
		errorCounter.Inc();
		throw;
	}
});

app.MapPut("/blockWallet", (string username, IWalletOperations walletOperations) =>
{
	try
	{
		securityCounter.Inc();
		return walletOperations.BlockByOwner(username);
	}
	catch (global::System.Exception)
	{
		errorCounter.Inc();
		throw;
	}
});

app.MapPut("/unlockWallet", (Wallet wallet, IWalletOperations walletOperations ) =>
{
	try
	{
		securityCounter.Inc();
		walletOperations.Unlock(wallet);
	}
	catch (global::System.Exception)
	{
		errorCounter.Inc();
		throw;
	}
});


app.UseMetricServer(5000);
app.Run();
