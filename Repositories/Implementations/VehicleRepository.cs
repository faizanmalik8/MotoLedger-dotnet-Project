using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MotoLedger.Models.Entities;
using MotoLedger.Repositories.Interfaces;

namespace MotoLedger.Repositories.Implementations
{
    public class VehicleRepository:IVehicleRepository
    {
        private readonly string _connectionString;
        public VehicleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection not found in appsettings.json.");
        }
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            const string query = "select * from Vehicles Order by Id DESC";
            return await conn.QueryAsync<Vehicle>(query);
        }
        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            SqlConnection conn= new SqlConnection(_connectionString);
            string query = "select * from Vehicles where Status='Availabe' Order by Id DESC";
            return await conn.QueryAsync<Vehicle>(query);
        }
        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            SqlConnection conn=new SqlConnection(_connectionString);
            string query = "Select * from Vehicles where Id=@Id";
            return await conn.QueryFirstOrDefaultAsync<Vehicle>(query, new { Id=id });
        }
        public async Task<int> AddAsync(Vehicle vehicle)
        {
            SqlConnection conn=new SqlConnection(_connectionString);
            string query = @"insert into Vehicles(RegistrationNumber, Make, Model, BasePurchasePrice, PurchaseDate, Status) values
            (@RegistrationNumber, @Make, @Model, @BasePurchasePrice,@PurchaseDate, @Status);
            SELECT CAST(SCOPE_IDENTITY() as int);";
            return await conn.ExecuteScalarAsync<int>(query, vehicle);
        }
        public async Task<bool> UpdateAsync(Vehicle vehicle)
        {
            SqlConnection conn= new SqlConnection (_connectionString);
            string query = @"Update Vehicle set RegistrationNumber=@RegistrationNumber,
            Make=@Make,Model=@Model,BasePurchasePrice=@BasePurchasePrice,PurchaseDate=@PurchaseDate,Status=@Status
            where Id=@Id";
            var res=await conn.ExecuteAsync(query, vehicle);
            return res > 0;
        }
        public async Task<bool>DeleteAsync(int id)
        {
            SqlConnection conn=new SqlConnection (_connectionString);
            string query = "delete from Vehicles where Id=@Id";
            var res = await conn.ExecuteAsync(query, new { Id = id });
            return res > 0;
        }
        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            SqlConnection conn= new SqlConnection (_connectionString);
            string query = "Update Vehicles Set Status=@Status Where Id=@Id";
            var res = await conn.ExecuteAsync(query, new { Id = id, Status = status });
            return res > 0;
        }

    }
}
