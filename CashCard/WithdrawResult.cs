namespace CashCard
{
    /// <summary>
    /// Results of a Withdraw operation
    /// </summary>
    public enum WithdrawResult
    {
        Success = 1,
        IncorrectPin = 2,
        InsufficientFunds = 3,
        InvalidAmount = 4,
    }
}
