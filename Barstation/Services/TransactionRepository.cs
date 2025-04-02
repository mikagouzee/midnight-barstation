// File: TransactionRepository.cs
using Barstation.Models;

namespace Barstation.Services
{
    public interface ITransactionRepository
    {
        List<Transaction> GetAll();
        void Add(Transaction transaction);
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly List<Transaction> _transactionHistory = new List<Transaction>();

        public List<Transaction> GetAll() => _transactionHistory;

        public void Add(Transaction transaction) => _transactionHistory.Add(transaction);
    }

}