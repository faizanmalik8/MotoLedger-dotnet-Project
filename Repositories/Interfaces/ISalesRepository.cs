using MotoLedger.Models.Entities;

namespace MotoLedger.Repositories.Interfaces
{
    public interface ISalesRepository
    {
        Task<IEnumerable<SalesTransaction>> GetAllSalesAsync();
        Task<SalesTransaction?> GetByIdAsync(int id);
        Task<SalesTransaction?> GetByVehicleIdAsync(int vehicleId);
        Task<int> ExecuteSaleAsync(SalesTransaction transaction);
    }
}
