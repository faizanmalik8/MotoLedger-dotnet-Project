using Microsoft.AspNetCore.Mvc;
using MotoLedger.Repositories.Interfaces;


namespace MotoLedger.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesRepository _salesrepo;
        public SalesController(ISalesRepository salesrepo)
        {
            _salesrepo = salesrepo;
        }
        // GET: /Sales/Checkout
        public IActionResult Checkout()
        {
            return View();
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
