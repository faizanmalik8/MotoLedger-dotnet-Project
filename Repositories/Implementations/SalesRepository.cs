using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MotoLedger.Models.Entities;
using MotoLedger.Repositories.Interfaces;

namespace MotoLedger.Repositories.Implementations
{
    public class SalesRepository : ISalesRepository
    {
        private readonly string _connectionString;

        public SalesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection not found in appsettings.json.");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<SalesTransaction>> GetAllSalesAsync()
        {
            const string sql = "SELECT * FROM SalesTransactions ORDER BY SaleDate DESC";
            using var connection = CreateConnection();
            return await connection.QueryAsync<SalesTransaction>(sql);
        }

        public async Task<SalesTransaction?> GetByIdAsync(int id)
        {
            const string sql = "SELECT * FROM SalesTransactions WHERE Id = @Id";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SalesTransaction>(sql, new { Id = id });
        }

        public async Task<SalesTransaction?> GetByVehicleIdAsync(int vehicleId)
        {
            const string sql = "SELECT * FROM SalesTransactions WHERE VehicleId = @VehicleId";
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SalesTransaction>(sql, new { VehicleId = vehicleId });
        }

        public async Task<int> ExecuteSaleAsync(SalesTransaction transaction)
        {
            using var connection = (SqlConnection)CreateConnection();
            await connection.OpenAsync();
            using var sqlTransaction = connection.BeginTransaction();

            try
            {
                // 1. Calculate Grand Total Cost: BasePurchasePrice + Sum of All Expenses
                const string costSql = @"
                    SELECT v.BasePurchasePrice + ISNULL(SUM(e.Cost), 0)
                    FROM Vehicles v
                    LEFT JOIN VehicleExpenses e ON v.Id = e.VehicleId
                    WHERE v.Id = @VehicleId
                    GROUP BY v.BasePurchasePrice";

                var grandTotalCost = await connection.ExecuteScalarAsync<decimal>(
                    costSql,
                    new { VehicleId = transaction.VehicleId },
                    transaction: sqlTransaction);

                // 2. Net Profit = SalePrice - Grand Total Cost
                transaction.CalculatedProfit = transaction.SalePrice - grandTotalCost;

                // 3. Insert SalesTransaction record
                const string insertSql = @"
                    INSERT INTO SalesTransactions (VehicleId, SalePrice, SaleDate, CustomerName, CustomerPhone, PaymentMethod, CalculatedProfit)
                    VALUES (@VehicleId, @SalePrice, @SaleDate, @CustomerName, @CustomerPhone, @PaymentMethod, @CalculatedProfit);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                var transactionId = await connection.ExecuteScalarAsync<int>(
                    insertSql,
                    transaction,
                    transaction: sqlTransaction);

                // 4. Lock Vehicle status to 'Sold'
                const string updateVehicleSql = "UPDATE Vehicles SET Status = 'Sold' WHERE Id = @VehicleId";
                await connection.ExecuteAsync(
                    updateVehicleSql,
                    new { VehicleId = transaction.VehicleId },
                    transaction: sqlTransaction);

                // 5. Commit all changes atomically
                sqlTransaction.Commit();
                return transactionId;
            }
            catch
            {
                sqlTransaction.Rollback();
                throw;
            }
        }
    }
}