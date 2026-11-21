namespace MotoLedger.Models.Entities
{
    public class VehicleExpense
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string ExpenseType { get; set; }= string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public decimal Cost {  get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public string? Description {  get; set; } 


    }
}
