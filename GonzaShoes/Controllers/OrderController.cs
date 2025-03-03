using System.Diagnostics;
using GonzaShoes.Model.Entities.Order;
using GonzaShoes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class OrderController : BackendController
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            return base.OnActionExecutionAsync(context, next);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            //var products = await _context.Products.ToListAsync();
            //return View(products);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                //_context.Orders.Add(order);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(History));
                return View();
            }
            return View(order);
        }

        public async Task<IActionResult> History()
        {
            //var orders = await _context.Orders
            //    .Include(o => o.OrderItems)
            //    .ThenInclude(oi => oi.Product)
            //    .ToListAsync();
            //return View(orders);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
