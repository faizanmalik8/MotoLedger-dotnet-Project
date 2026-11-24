using Microsoft.AspNetCore.Mvc;
using MotoLedger.Repositories.Interfaces;


namespace MotoLedger.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesRepository _salesrepo;
        private readonly IVehicleRepository _vehiclerepo;
        private readonly IExpenseRepository _expenserepo;
        public SalesController(ISalesRepository salesrepo, IVehicleRepository vehiclerepo, IExpenseRepository expenserepo)
        {
            _salesrepo = salesrepo;
            _vehiclerepo = vehiclerepo;
            _expenserepo = expenserepo;
        }
        // GET: /Sales/Checkout
        public async Task<IActionResult> Checkout(int? vehicleId)
        {
            var availablevehicles = await _vehiclerepo.GetAvailableVehiclesAsync();
            ViewBag.Availablevehicles = availablevehicles;
            ViewBag.SelectedVehicleId=vehicleId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicleFinancials(int vehicleId)
        {
            var vehicle=await _vehiclerepo.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return Json(new {success="false", message="Vehicle Not Found"});
            }
            var workshopexpenses = await _expenserepo.GetTotalExpensesForVehicleAsync(vehicleId);
            var totalCostBasis = vehicle.BasePurchasePrice + workshopexpenses;
            return Json(new
            {
                success = true,
                baseprice = vehicle.BasePurchasePrice,
                workshopExpenses = workshopexpenses,
                totalcost = totalCostBasis,

            });
            
        }

        // GET: /Sales/Ledger
        public IActionResult Ledger()
        {
            return View();
        }

        // GET: /Sales/Invoice/5
        public IActionResult Invoice(int? id)
        {
            return View();
        }
    }
}
