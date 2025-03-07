using System.Diagnostics;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.DTOs.Order;
using GonzaShoes.Models;
using GonzaShoes.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class BrandController : BackendController
    {
        private readonly IBrandService brandService;

        private readonly ILogger<BrandController> _logger;
        
        public BrandController(IBrandService brandService, ILogger<BrandController> logger)
        {
            this.brandService = brandService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.brandService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        public async Task<IActionResult> IndexAsync([FromQuery] BrandSearchDTO searchDTO)
        {
            var users = await brandService.GetBrandsAsync(searchDTO);

            return View(users);
        }


        public async Task<IActionResult> EditAsync(int id)
        {
            if (id > 0)
            {
                var user = await this.brandService.GetBrandByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new BrandDTO());
        }

        public async Task<IActionResult> DuplicateAsync(int id)
        {
            if (id > 0)
            {
                var modelProduct = await this.brandService.GetBrandByIdAsync(id);
                if (modelProduct == null)
                    return NotFound();

                BrandDTO newModel = (BrandDTO)modelProduct.Clone();
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
                ValidationResultDTO validationResultDTO = await this.brandService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(BrandDTO brand)
        {
            if (ModelState.IsValid)
            {
                ValidationResultDTO validationResultDTO = await this.brandService.SaveBrandAsync(brand);
                if (validationResultDTO.HasErrors)
                {
                    ModelState.AddModelError(string.Empty, validationResultDTO.GetErrorMessages());
                    return View("Edit", brand);
                }
                return RedirectToAction("Index");
            }
            return View("Edit", brand);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
