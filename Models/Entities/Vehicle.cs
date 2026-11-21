namespace MotoLedger.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Make {  get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal BasePurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }= DateTime.Now;
        public string Status { get; set; } = "Availabe";

        public List<VehicleExpense> Expenses { get; set; } = new();
        public SalesTransaction? SalesTransaction { get; set; }
    }
}
