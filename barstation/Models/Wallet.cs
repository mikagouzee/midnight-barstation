namespace Barstation.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public required string Owner { get; set; }
        public decimal CurrentValue { get; set; }
        public List<Transaction> LastTransactions { get; set; } = new();
        public DateTime LastUpdate { get; set; }
        public bool IsBlocked { get; set; } = false;

    } 
}