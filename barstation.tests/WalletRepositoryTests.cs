// File: Project.Tests/WalletRepositoryTests.cs
using Barstation.Models;
using Barstation.Services;
using System.Collections.Generic;
using Xunit;

namespace Barstation.barstation.tests
{
    public class WalletRepositoryTests
    {
        private readonly WalletRepository _walletRepository;

        public WalletRepositoryTests()
        {
            _walletRepository = new WalletRepository();
        }

        [Fact]
        public void Add_ShouldAddWallet()
        {
            // Arrange
            var wallet = new Wallet { Id = 2, Owner = "User3", CurrentValue = 200.00M };

            // Act
            _walletRepository.Add(wallet);

            // Assert
            Assert.Contains(wallet, _walletRepository.GetAll());
        }

        [Fact]
        public void GetByOwner_ShouldReturnWallet_WhenWalletExists()
        {
            // Arrange
            var wallet = new Wallet { Id = 2, Owner = "User3", CurrentValue = 200.00M };
            _walletRepository.Add(wallet);

            // Act
            var result = _walletRepository.TryGetByOwner("User3");

            // Assert
            Assert.Equal(wallet, result);
        }

        [Fact]
        public void GetByOwner_ShouldReturnNull_WhenWalletDoesNotExist()
        {
            // Act
            var result = _walletRepository.TryGetByOwner("NonExistentUser");

            // Assert
            Assert.Null(result);
        }
    }

}