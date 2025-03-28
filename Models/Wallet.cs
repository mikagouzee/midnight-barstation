public class Wallet
{
    public int Id { get; set; }
    public string Owner { get; set; }
    public decimal CurrentValue { get; set; }
    public List<Transaction> LastTransactions { get; set; } = new();
    public DateTime LastUpdate { get; set; }
}