namespace BudgetAPI.Models
{
    public class Outcome
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateReceived { get; set; }
    }
}
