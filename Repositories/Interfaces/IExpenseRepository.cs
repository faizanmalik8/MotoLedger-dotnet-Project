using MotoLedger.Models.Entities;

namespace MotoLedger.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<VehicleExpense>> GetAllAsync();
        Task<IEnumerable<VehicleExpense>> GetByVehicleIdAsync(int vehicleId);
        Task<int> AddAsync(VehicleExpense expense);
        Task<decimal> GetTotalExpensesForVehicleAsync(int vehicleId);
        Task<decimal> GetGrandTotalCostAsync(int vehicleId);
    }
}
