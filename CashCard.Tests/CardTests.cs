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
        [Fact]
        public void CanCreate()
        {
            var card = new Card(1234);

            card.Balance.Should().Be(0m);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public void CanTopUp(decimal initialBalance)
        {
            var card = new Card(1234, initialBalance);

            var amount = 100;

            card.TopUp(amount);
            card.Balance.Should().Be(initialBalance + amount);
        }

        /// <remarks>
        /// Assumption made that a TopUp operation should only be possible with a positive amount.
        /// Negatives make sense to disallow but requirements say 'arbitrary' which may include zero. - verify with Client
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void TopUpNonPositiveAmountThrowsException(decimal amount)
        {
            var card = new Card(1234);

            Action topUpAction = () => card.TopUp(amount);

            topUpAction.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(100, 50, true)]
        [InlineData(50, 100, false)]
        [InlineData(0, 1, false)]
        public void WithdrawSucceedsWithAvailableFunds(decimal initialAmount, decimal withdrawalAmount, bool shouldSucceed)
        {
            var card = new Card(1234, initialAmount);

            var result = card.Withdraw(withdrawalAmount, 1234);

            result.Should().Be(shouldSucceed);
        }

        [Fact]
        public void WithdrawFailsWithoutMatchingPin()
        {
            var card = new Card(1234, 100);

            var result = card.Withdraw(50, 0000);

            result.Should().BeFalse();
        }
    }
}
