public class Transaction
{
    public DateTime Timestamp { get; set; }
    public string Type { get; set; } // "Payment" or "Refill"
    public decimal Amount { get; set; }
}