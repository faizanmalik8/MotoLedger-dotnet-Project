using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MotoLedger.Models.Entities;
using MotoLedger.Repositories.Interfaces;

namespace MotoLedger.Repositories.Implementations
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly string _connectionString;

        public ExpenseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection not found in appsettings.json.");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<VehicleExpense>> GetAllAsync()
        {
            const string sql = "SELECT * FROM VehicleExpenses ORDER BY ExpenseDate DESC";
            using var connection = CreateConnection();
            return await connection.QueryAsync<VehicleExpense>(sql);
        }

        public async Task<IEnumerable<VehicleExpense>> GetByVehicleIdAsync(int vehicleId)
        {
            const string sql = "SELECT * FROM VehicleExpenses WHERE VehicleId = @VehicleId ORDER BY ExpenseDate DESC";
            using var connection = CreateConnection();
            return await connection.QueryAsync<VehicleExpense>(sql, new { VehicleId = vehicleId });
        }

        public async Task<int> AddAsync(VehicleExpense expense)
        {
            const string sql = @"
                INSERT INTO VehicleExpenses (VehicleId, ExpenseType, VendorName, Cost, ExpenseDate, Description)
                VALUES (@VehicleId, @ExpenseType, @VendorName, @Cost, @ExpenseDate, @Description);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, expense);
        }

        public async Task<decimal> GetTotalExpensesForVehicleAsync(int vehicleId)
        {
            const string sql = "SELECT ISNULL(SUM(Cost), 0) FROM VehicleExpenses WHERE VehicleId = @VehicleId";
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(sql, new { VehicleId = vehicleId });
        }

        public async Task<decimal> GetGrandTotalCostAsync(int vehicleId)
        {
            // Grand Total Cost = BasePurchasePrice + Sum of all expenses on this car
            const string sql = @"
                SELECT 
                    v.BasePurchasePrice + ISNULL(SUM(e.Cost), 0)
                FROM Vehicles v
                LEFT JOIN VehicleExpenses e ON v.Id = e.VehicleId
                WHERE v.Id = @VehicleId
                GROUP BY v.BasePurchasePrice";

            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(sql, new { VehicleId = vehicleId });
        }
    }
}