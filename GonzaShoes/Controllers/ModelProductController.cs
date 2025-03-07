using System.Diagnostics;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Color;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Models;
using GonzaShoes.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class ModelProductController : BackendController
    {
        private readonly IModelProductService modelProductService;
        private readonly IBrandService brandService;

        private readonly ILogger<ModelProductController> _logger;

        public ModelProductController(IModelProductService modelProductService, IBrandService brandService, ILogger<ModelProductController> logger)
        {
            this.modelProductService = modelProductService;
            this.brandService = brandService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.modelProductService.SetCurrentUser(userId);
            this.brandService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        private async Task GetFiltersAsync()
        {
            ViewBag.Brands = await brandService.GetNameIdDTOsAsync();
        }

        public async Task<IActionResult> IndexAsync([FromQuery] ModelProductSearchDTO searchDTO)
        {
            var users = await modelProductService.GetModelProductsAsync(searchDTO);
            await GetFiltersAsync();

            return View(users);
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            await GetFiltersAsync();

            if (id > 0)
            {
                var user = await this.modelProductService.GetModelProductByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new ModelProductDTO());
        }

        public async Task<IActionResult> DuplicateAsync(int id)
        {
            await GetFiltersAsync();

            if (id > 0)
            {
                var modelProduct = await this.modelProductService.GetModelProductByIdAsync(id);
                if (modelProduct == null)
                    return NotFound();

                ModelProductDTO newModel = (ModelProductDTO)modelProduct.Clone();
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
                ValidationResultDTO validationResultDTO = await this.modelProductService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ModelProductDTO dto)
        {
            if (ModelState.IsValid)
            {
                ValidationResultDTO validationResultDTO = await this.modelProductService.SaveModelProductAsync(dto);
                if (validationResultDTO.HasErrors)
                {
                    ModelState.AddModelError(string.Empty, validationResultDTO.GetErrorMessages());

                    await GetFiltersAsync();
                    return View("Edit", dto);
                }
                return RedirectToAction("Index");
            }

            await GetFiltersAsync();
            return View("Edit", dto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
