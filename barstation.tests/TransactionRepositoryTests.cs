// File: Project.Tests/TransactionRepositoryTests.cs
using Barstation.Models;
using Barstation.Services;
using System.Collections.Generic;
using Xunit;

namespace Barstation.barstation.tests
{
    public class TransactionRepositoryTests
    {
        private readonly TransactionRepository _transactionRepository;

        public TransactionRepositoryTests()
        {
            _transactionRepository = new TransactionRepository();
        }

        [Fact]
        public void Add_ShouldAddTransaction()
        {
            // Arrange
            var transaction = new Transaction { Timestamp = DateTime.UtcNow, Type = TransactionType.Refill, Amount = 50.00M };

            // Act
            _transactionRepository.Add(transaction);

            // Assert
            Assert.Contains(transaction, _transactionRepository.GetAll());
        }

        [Fact]
        public void GetAll_ShouldReturnAllTransactions()
        {
            // Arrange
            var transaction1 = new Transaction { Timestamp = DateTime.UtcNow, Type = TransactionType.Refill, Amount = 50.00M };
            var transaction2 = new Transaction { Timestamp = DateTime.UtcNow, Type = TransactionType.Payment, Amount = 30.00M };
            _transactionRepository.Add(transaction1);
            _transactionRepository.Add(transaction2);

            // Act
            var transactions = _transactionRepository.GetAll();

            // Assert
            Assert.Contains(transaction1, transactions);
            Assert.Contains(transaction2, transactions);
        }
    }

}