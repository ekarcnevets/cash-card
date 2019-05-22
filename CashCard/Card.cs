using System;

namespace CashCard
{
    /// <summary>A virtual Cash Card supporting use in many places at the same time.</summary>
    /// <remarks>
    /// Assumption made that TopUp and Withdraw operations should only be possible with a positive amount.
    /// Negatives make sense to disallow but requirements say 'arbitrary' which may include zero. - TODO: verify with Client.
    /// </remarks>
    public class Card
    {
        private readonly object _balanceLock = new object();

        /// <summary>
        /// The secure PIN of the Card.
        /// </summary>
        private readonly uint _pin;

        /// <summary>
        /// The number of the Card.
        /// </summary>
        /// <remarks>
        /// Assumption that an identifier is required - TODO: verify with Client.
        /// Chose string for this rather than an unsigned long as it's simpler to do operations like check specific digits for determining provider.
        /// </remarks>
        public string Number { get; private set; }

        /// <summary>
        /// The current balance on the Card.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Creates a Card with a secure PIN number and an initial balance of zero.
        /// </summary>
        /// <param name="pin">The PIN to assign to the Card.</param>
        /// <param name="initialBalance">The initial balance on the Card.</param
        public Card(string number, uint pin, decimal initialBalance = 0m)
        {
            _pin = pin;

            Number = number;
            Balance = initialBalance;
        }

        /// <summary>
        /// Withdraw an amount from the Card, providing the matching PIN.
        /// </summary>
        /// <param name="amount">The positive amount to withdraw.</param>
        /// <param name="pin">The provided PIN to authenticate against the Cards PIN.</param>
        public WithdrawResult Withdraw(decimal amount, uint pin)
        {
            if (amount <= 0)
                return WithdrawResult.InvalidAmount;

            if (pin != _pin)
                return WithdrawResult.IncorrectPin;

            lock (_balanceLock)
            {
                if (amount > Balance)
                    return WithdrawResult.InsufficientFunds;

                Balance -= amount;
                return WithdrawResult.Success;
            }
        }

        /// <summary>
        /// Top up the Card by an arbitrary positive amount.
        /// </summary>
        /// <param name="amount">The amount to top up by.</param>
        public TopUpResult TopUp(decimal amount)
        {
            if (amount <= 0)
                return TopUpResult.InvalidAmount;

            lock (_balanceLock)
            {
                Balance += amount;
                return TopUpResult.Success;
            }
        }
    }
}
