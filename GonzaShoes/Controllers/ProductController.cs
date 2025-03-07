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
    public class ProductController : BackendController
    {
        private readonly IProductService productService;
        private readonly IModelProductService modelProductService;
        private readonly IBrandService brandService;
        private readonly IColorService colorService;
        private readonly ISizeService sizeService;

        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService,
                                 IModelProductService modelProductService,
                                 IBrandService brandService,
                                 IColorService colorService,
                                 ISizeService sizeService,
                                 ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.modelProductService = modelProductService;
            this.brandService = brandService;
            this.colorService = colorService;
            this.sizeService = sizeService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.productService.SetCurrentUser(userId);
            this.modelProductService.SetCurrentUser(userId);
            this.brandService.SetCurrentUser(userId);
            this.colorService.SetCurrentUser(userId);
            this.sizeService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        private async Task GetFiltersAsync(bool findModelProducts = true)
        {
            ViewBag.Brands = await brandService.GetNameIdDTOsAsync();
            if (findModelProducts)
                ViewBag.ModelProducts = await modelProductService.GetNameIdDTOsAsync();
            ViewBag.Colors = await colorService.GetNameIdDTOsAsync();
            ViewBag.Sizes = await sizeService.GetNameIdDTOsAsync();
        }

        public async Task<IActionResult> IndexAsync([FromQuery] ProductSearchDTO searchDTO)
        {
            var users = await productService.GetProductsAsync(searchDTO);
            await GetFiltersAsync();

            return View(users);
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            await GetFiltersAsync(false);

            if (id > 0)
            {
                var user = await this.productService.GetProductByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new ProductDTO());
        }

        public async Task<IActionResult> DuplicateAsync(int id)
        {
            await GetFiltersAsync(false);

            if (id > 0)
            {
                var modelProduct = await this.productService.GetProductByIdAsync(id);
                if (modelProduct == null)
                    return NotFound();

                ProductDTO newModel = (ProductDTO)modelProduct.Clone();
                newModel.Id = 0;
                newModel.CreatedUserId = userId;
                newModel.DateCreated = DateTime.Now;
                newModel.DateUpdated = null;
                newModel.UpdatedUserId = null;

                return View("Edit", newModel);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateStatusAsync(int id, bool isActive)
        {
            if (id > 0)
            {
                ValidationResultDTO validationResultDTO = await this.productService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductDTO dto)
        {
            if (ModelState.IsValid)
            {
                ValidationResultDTO validationResultDTO = await this.productService.SaveProductAsync(dto);
                if (validationResultDTO.HasErrors)
                {
                    ModelState.AddModelError(string.Empty, validationResultDTO.GetErrorMessages());
                    await GetFiltersAsync(false);
                    return View("Edit", dto);
                }
                return RedirectToAction("Index");
            }
            await GetFiltersAsync(false);
            return View("Edit", dto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
