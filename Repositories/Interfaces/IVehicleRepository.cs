using MotoLedger.Models.Entities;

namespace MotoLedger.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<int> AddAsync(Vehicle vehicle);
        Task<bool> UpdateAsync(Vehicle vehicle);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int vehicleId, string status);
    }
}
