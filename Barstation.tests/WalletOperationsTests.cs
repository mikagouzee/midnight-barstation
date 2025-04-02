// File: Project.Tests/WalletOperationsTests.cs
using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Barstation.Models;
using Barstation.Services;

namespace Barstation.barstation.tests
{
    public class WalletOperationsTests
    {
        private readonly Mock<IWalletRepository> _mockWalletRepository;
        private readonly Mock<ITransactionRepository> _mockTransactionRepository;
        private readonly WalletOperations _walletOperations;

        public WalletOperationsTests()
        {
            _mockWalletRepository = new Mock<IWalletRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _walletOperations = new WalletOperations(_mockWalletRepository.Object, _mockTransactionRepository.Object);
        }

        [Fact]
        public void AddValue_ShouldReturnBadRequest_WhenAmountIsZeroOrLess()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M };

            // Act
            var result = _walletOperations.AddValue(wallet, 0);

            // Assert
            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public void AddValue_ShouldReturnBadRequest_WhenWalletIsBlocked()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M, IsBlocked = true };

            // Act
            var result = _walletOperations.AddValue(wallet, 50);

            // Assert
            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public void AddValue_ShouldUpdateWalletBalance_WhenAmountIsValid()
        {
            // Arrange
            Wallet wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M, LastTransactions = new List<Transaction>() };
            _mockWalletRepository.Setup(repo => repo.Update(wallet)).Returns(wallet);

            // Act
            var result = _walletOperations.AddValue(wallet, 50);

            // Assert
            Assert.IsType<Ok<Wallet>>(result);
            Assert.Equal(150.00M, wallet.CurrentValue);
        }

        [Fact]
        public void SubtractValue_ShouldReturnBadRequest_WhenAmountIsZeroOrLess()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M };

            // Act
            var result = _walletOperations.SubtractValue(wallet, 0);

            // Assert
            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public void SubtractValue_ShouldReturnBadRequest_WhenWalletIsBlocked()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M, IsBlocked = true };

            // Act
            var result = _walletOperations.SubtractValue(wallet, 50);

            // Assert
            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public void SubtractValue_ShouldReturnBadRequest_WhenInsufficientBalance()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M };

            // Act
            var result = _walletOperations.SubtractValue(wallet, 150);

            // Assert
            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public void SubtractValue_ShouldUpdateWalletBalance_WhenAmountIsValid()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M, LastTransactions = new List<Transaction>() };
            _mockWalletRepository.Setup(repo => repo.Update(wallet)).Returns(wallet);

            // Act
            var result = _walletOperations.SubtractValue(wallet, 50);

            // Assert
            Assert.IsType<Ok<Wallet>>(result);
            Assert.Equal(50.00M, wallet.CurrentValue);
        }

        [Fact]
        public void Create_ShouldReturnBadRequest_WhenWalletAlreadyExists()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M };
            _mockWalletRepository.Setup(repo => repo.TryGetByOwner("User1")).Returns(wallet);

            // Act
            var result = _walletOperations.Create(wallet);

            // Assert
            Assert.IsType<BadRequest<string>>(result);
        }

        [Fact]
        public void Create_ShouldAddWallet_WhenWalletDoesNotExist()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M };
            //_mockWalletRepository.Setup(repo => repo.TryGetByOwner("User1")).Returns((Wallet)null);

            // Act
            var result = _walletOperations.Create(wallet);

            // Assert
            Assert.IsType<Ok<Wallet>>(result);
            _mockWalletRepository.Verify(repo => repo.Add(wallet), Times.Once);
        }

        [Fact]
        public void BlockByOwner_ShouldBlockWallet()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M };
            //_mockWalletRepository.Setup(repo => repo.TryGetByOwner("User1")).Returns(wallet);

            // Act
            var result = _walletOperations.BlockByOwner(wallet.Owner);

            // Assert
            Assert.IsType<Ok>(result);
            _mockWalletRepository.Verify(repo => repo.BlockByOwner(wallet.Owner), Times.Once);
        }

        [Fact]
        public void Unlock_ShouldUnlockWallet()
        {
            // Arrange
            var wallet = new Wallet { Owner = "User1", CurrentValue = 100.00M, IsBlocked = true };
            _mockWalletRepository.Setup(repo => repo.Unlock(wallet))
                .Callback(() => wallet.IsBlocked = false)
                .Returns(wallet);

            // Act
            var result = _walletOperations.Unlock(wallet);

            // Assert
            Assert.IsType<Ok<Wallet>>(result);
            Assert.False(wallet.IsBlocked);
        }
    }
}
