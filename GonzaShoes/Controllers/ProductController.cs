using System.Diagnostics;
using GonzaShoes.Model.Entities.Product;
using GonzaShoes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class ProductController : BackendController
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
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

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(product);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return View();
            }
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
