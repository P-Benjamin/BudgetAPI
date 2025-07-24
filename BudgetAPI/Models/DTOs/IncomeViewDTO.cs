namespace BudgetAPI.Models.DTOs
{
    public class IncomeViewDTO
    {
        public int Id { get; set; }
        public string SourceName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateReceived { get; set; }
    }
}
