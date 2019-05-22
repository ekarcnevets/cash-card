using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CashCard.Tests
{
    /// <summary>
    /// Tests for the Card class.
    /// </summary>
    /// <remarks>
    /// It would be nice to test for race conditions here - but there isn't really a nice way to do it.
    /// One possible option would be to introduce a small random delay before a TopUp or Withdraw operation, then run them simultaneously.
    /// There are testing tools that allow you to churn tests a specific number of times or until they fail to help reproduce such bugs.
    /// </remarks>
    public class CardTests
    {
        private readonly string _number = "1234567812345678";
        private readonly uint _pin = 1234;

        [Fact]
        public void CanCreate()
        {
            var card = new Card(_number, _pin);

            card.Balance.Should().Be(0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public void CanTopUp(decimal initialBalance)
        {
            var card = new Card(_number, _pin, initialBalance);

            var amount = 100;

            var result = card.TopUp(amount);

            result.Should().Be(TopUpResult.Success);
            card.Balance.Should().Be(initialBalance + amount);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TopUpNonPositiveAmountFails(decimal amount)
        {
            var card = new Card(_number, _pin);

            var result = card.TopUp(amount);

            result.Should().Be(TopUpResult.InvalidAmount);

            // Balance should not be affected.
            card.Balance.Should().Be(0);
        }

        [Theory]
        [InlineData(100, 50, WithdrawResult.Success)]
        [InlineData(50, 100, WithdrawResult.InsufficientFunds)]
        [InlineData(0, 1, WithdrawResult.InsufficientFunds)]
        public void WithdrawSucceedsWithAvailableFunds(decimal initialAmount, decimal withdrawalAmount, WithdrawResult expectedResult)
        {
            var card = new Card(_number, _pin, initialAmount);

            var actualResult = card.Withdraw(withdrawalAmount, _pin);

            actualResult.Should().Be(expectedResult);

            // Balance should only be affected if the withdraw succeeded.
            if (actualResult == WithdrawResult.Success)
                card.Balance.Should().Be(initialAmount - withdrawalAmount);
            else
                card.Balance.Should().Be(initialAmount);
        }

        [Fact]
        public void WithdrawNonPositiveAmountFails()
        {
            var card = new Card(_number, _pin, 100);

            var actualResult = card.Withdraw(-50, _pin);

            actualResult.Should().Be(WithdrawResult.InvalidAmount);

            // Balance should not be affected.
            card.Balance.Should().Be(100);
        }

        [Fact]
        public void WithdrawFailsWithoutMatchingPin()
        {
            var card = new Card(_number, _pin, 100);

            var result = card.Withdraw(50, 0000);

            result.Should().Be(WithdrawResult.IncorrectPin);

            // Balance should not be affected.
            card.Balance.Should().Be(100);
        }
    }
}
