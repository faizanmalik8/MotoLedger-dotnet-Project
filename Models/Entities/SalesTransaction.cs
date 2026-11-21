namespace MotoLedger.Models.Entities
{
    public class SalesTransaction
    {
        public int Id { get; set; }
        public int VehicleId {  get; set; }
        public decimal SalePrice { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; }= string.Empty;
        public string PaymentMethod {  get; set; } = string.Empty;
        public decimal CalculatedProfit { get; set; }

    }
}
