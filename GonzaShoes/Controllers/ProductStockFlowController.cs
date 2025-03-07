using System.Diagnostics;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Models;
using GonzaShoes.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class ProductStockFlowController : BackendController
    {
        private readonly IProductStockFlowService productStockFlowService;

        private readonly ILogger<ProductStockFlowController> _logger;

        public ProductStockFlowController(IProductStockFlowService productStockFlowService, ILogger<ProductStockFlowController> logger)
        {
            this.productStockFlowService = productStockFlowService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.productStockFlowService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        public async Task<IActionResult> IndexAsync()
        {
            var users = await productStockFlowService.GetProductStockFlowsAsync();

            return View(users);
        }

        public async Task<IActionResult> EditAsync(int id)
        {

            if (id > 0)
            {
                var user = await this.productStockFlowService.GetProductStockFlowByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new ProductStockFlowDTO());
        }

        public async Task<IActionResult> UpdateStatusAsync(int id, bool isActive)
        {
            if (id > 0)
            {
                ValidationResultDTO validationResultDTO = await this.productStockFlowService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Save(ProductDTO dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ValidationResultDTO validationResultDTO = await this.productService.SaveProductAsync(dto);
        //        if (validationResultDTO.HasErrors)
        //        {
        //            ModelState.AddModelError(string.Empty, validationResultDTO.GetErrorMessages());
        //            await GetFiltersAsync(false);
        //            return View("Edit", dto);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    await GetFiltersAsync(false);
        //    return View("Edit", dto);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
