// File: WalletOperations.cs
using Barstation.Models;

namespace Barstation.Services
{

    public interface IWalletOperations
    {
        IResult AddValue(Wallet wallet, decimal amount);
        IResult SubtractValue(Wallet wallet, decimal amount);


        IResult Create(Wallet wallet);

        IResult BlockByOwner(string username);

        IResult Unlock(Wallet wallet);
    }

    public class WalletOperations : IWalletOperations
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;

        public WalletOperations(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public IResult AddValue(Wallet wallet, decimal amount)
        {
            if (amount <= 0)
            {
                return Results.BadRequest("Amount must be greater than zero.");
            }
            
            if (wallet.IsBlocked)
            {
                return Results.BadRequest("Wallet is blocked.");
            }
            Console.WriteLine($"Processing wallet {wallet.Owner}");

            // Update wallet balance
            wallet.CurrentValue += amount;

            var current = new Transaction
            {
                Timestamp = DateTime.UtcNow,
                Type = TransactionType.Refill,
                Amount = amount
            };

            // Record the transaction
            wallet.LastTransactions.Add(current);
            // Update the lastUpdate field
            wallet.LastUpdate = DateTime.UtcNow;
            _transactionRepository.Add(current);
            wallet = _walletRepository.Update(wallet);

            Console.WriteLine($"Refill of {amount:C} processed. New balance: {wallet.CurrentValue:C}");

            return TypedResults.Ok(wallet);
        }

        public IResult SubtractValue(Wallet wallet, decimal amount)
        {
            if (amount <= 0)
            {
                return Results.BadRequest("Amount must be greater than zero.");
            }
            if (wallet.IsBlocked)
            {
                return Results.BadRequest("Wallet is blocked.");
            }
            Console.WriteLine($"Processing wallet {wallet.Owner}");

            if (wallet.CurrentValue >= amount)
            {
                wallet.CurrentValue -= amount;

                var transaction = new Transaction
                {
                    Timestamp = DateTime.UtcNow,
                    Type = TransactionType.Payment,
                    Amount = amount
                };
                
                wallet.LastTransactions.Add(transaction);
                
                wallet.LastUpdate = DateTime.UtcNow;
                wallet = _walletRepository.Update(wallet);

                _transactionRepository.Add(transaction);
                
                Console.WriteLine($"Payment of {amount:C} processed. New balance: {wallet.CurrentValue:C}");

                return TypedResults.Ok(wallet);
            }
            else
            {
                return Results.BadRequest("Insufficient balance.");
            }
        }

        public IResult Create(Wallet wallet)
        {
            var exists = _walletRepository.TryGetByOwner(wallet.Owner);

            if (exists != null)
            {
                return Results.BadRequest("Owner already has a wallet.");
            }
            _walletRepository.Add(wallet);
            return TypedResults.Ok(wallet);
        }

        public IResult BlockByOwner(string username)
        {
            _walletRepository.BlockByOwner(username);
            return TypedResults.Ok();
        }
        public IResult Unlock(Wallet wallet)
        {
            wallet = _walletRepository.Unlock(wallet);
            return TypedResults.Ok(wallet);
        }

    }

}