using System.Diagnostics;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Model.DTOs.Size;
using GonzaShoes.Models;
using GonzaShoes.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class SizeController : BackendController
    {
        private readonly ISizeService sizeService;

        private readonly ILogger<SizeController> _logger;

        public SizeController(ISizeService sizeService, ILogger<SizeController> logger)
        {
            this.sizeService = sizeService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.sizeService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        public async Task<IActionResult> IndexAsync([FromQuery] SizeSearchDTO searchDTO)
        {
            var users = await sizeService.GetSizesAsync(searchDTO);

            return View(users);
        }


        public async Task<IActionResult> EditAsync(int id)
        {
            if (id > 0)
            {
                var user = await this.sizeService.GetSizeByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new SizeDTO());
        }

        public async Task<IActionResult> DuplicateAsync(int id)
        {

            if (id > 0)
            {
                var modelProduct = await this.sizeService.GetSizeByIdAsync(id);
                if (modelProduct == null)
                    return NotFound();

                SizeDTO newModel = (SizeDTO)modelProduct.Clone();
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
                ValidationResultDTO validationResultDTO = await this.sizeService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(SizeDTO dto)
        {
            if (ModelState.IsValid)
            {
                ValidationResultDTO validationResultDTO = await this.sizeService.SaveSizeAsync(dto);
                if (validationResultDTO.HasErrors)
                {
                    ModelState.AddModelError(string.Empty, validationResultDTO.GetErrorMessages());
                    return View("Edit", dto);
                }
                return RedirectToAction("Index");
            }
            return View("Edit", dto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
