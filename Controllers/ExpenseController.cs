using Microsoft.AspNetCore.Mvc;
using MotoLedger.Repositories.Interfaces;

namespace MotoLedger.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenserepo;
        public ExpenseController(IExpenseRepository expenserepo)
        {
            _expenserepo = expenserepo;
        }
        // GET: /Expense/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: /Expense/History
        public IActionResult History()
        {
            return View();
        }
    }
}
