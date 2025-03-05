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
    public class ColorController : BackendController
    {
        private readonly IColorService colorService;

        private readonly ILogger<ColorController> _logger;

        public ColorController(IColorService colorService, ILogger<ColorController> logger)
        {
            this.colorService = colorService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.colorService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        public async Task<IActionResult> IndexAsync([FromQuery] ColorSearchDTO searchDTO)
        {
            var users = await colorService.GetColorsAsync(searchDTO);

            return View(users);
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            if (id > 0)
            {
                var user = await this.colorService.GetColorByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new ColorDTO());
        }

        public async Task<IActionResult> DuplicateAsync(int id)
        {
            if (id > 0)
            {
                var modelProduct = await this.colorService.GetColorByIdAsync(id);
                if (modelProduct == null)
                    return NotFound();

                // Crear un nuevo objeto sin ID para que se considere como un nuevo modelo
                var newModel = new ColorDTO
                {
                    Id = 0, // Aseguramos que sea un nuevo registro
                    Name = modelProduct.Name,
                    HexCode = modelProduct.HexCode
                };

                return View("Edit", newModel);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateStatusAsync(int id, bool isActive)
        {
            if (id > 0)
            {
                ValidationResultDTO validationResultDTO = await this.colorService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ColorDTO dto)
        {
            if (ModelState.IsValid)
            {
                ValidationResultDTO validationResultDTO = await this.colorService.SaveColorAsync(dto);
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
