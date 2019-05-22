using System;

namespace CashCard
{
    public class Card
    {
        /// <summary>
        /// The secure PIN of the Card.
        /// </summary>
        private readonly int _pin;

        /// <summary>
        /// The number of the Card.
        /// </summary>
        /// <remarks>Chose string for this rather than an unsigned long as it's simpler to do operations like check specific digits for determining provider</remarks>
        public string Number { get; set; }

        /// <summary>
        /// The current balance on the Card.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Creates a Card with a secure PIN number and an initial balance of zero.
        /// </summary>
        /// <param name="pin">The PIN to assign to the Card.</param>
        /// <param name="initialBalance">The initial balance on the Card.</param
        public Card(int pin, decimal initialBalance = 0m)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Withdraw an amount from the Card, providing the matching PIN.
        /// </summary>
        /// <param name="amount">The amount to withdraw.</param>
        /// <param name="pin">The PIN to authenticate against the Cards PIN.</param>
        /// <returns></returns>
        public bool Withdraw(decimal amount, int pin)
        {
            throw new NotImplementedException();
        }

        public void TopUp(decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
