namespace Barstation.Models
{
    public class Transaction
    {
        public DateTime Timestamp { get; set; }
        public TransactionType Type { get; set; } // "Payment" or "Refill"
        public decimal Amount { get; set; }
    }
    
    public enum TransactionType
    {
        Payment,
        Refill
    }
}

