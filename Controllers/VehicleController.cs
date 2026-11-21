using Microsoft.AspNetCore.Mvc;
using MotoLedger.Models.Entities;
using MotoLedger.Repositories.Interfaces;

namespace MotoLedger.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepo;

        public VehicleController(IVehicleRepository vehicleRepo)
        {
            _vehicleRepo = vehicleRepo;
        }

        // GET: /Vehicle
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleRepo.GetAllAsync();
            return View(vehicles);
        }

        // GET: /Vehicle/Create
        public IActionResult Create()
        {
            return View(new Vehicle { PurchaseDate = DateTime.Now });
        }

        // POST: /Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                await _vehicleRepo.AddAsync(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: /Vehicle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var vehicle = await _vehicleRepo.GetByIdAsync(id.Value);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }
    }
}
