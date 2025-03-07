using System.Diagnostics;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Size;
using GonzaShoes.Model.DTOs.User;
using GonzaShoes.Models;
using GonzaShoes.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class UserController : BackendController
    {
        private readonly IUserService userService;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.userService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        public async Task<IActionResult> IndexAsync([FromQuery] UserSearchDTO searchDTO)
        {
            var users = await userService.GetUsersAsync(searchDTO);

            return View(users);
        }


        public async Task<IActionResult> EditAsync(int id)
        {
            if (id > 0)
            {
                var user = await this.userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new UserDTO());
        }

        public async Task<IActionResult> DuplicateAsync(int id)
        {

            if (id > 0)
            {
                var modelProduct = await this.userService.GetUserByIdAsync(id);
                if (modelProduct == null)
                    return NotFound();

                UserDTO newModel = (UserDTO)modelProduct.Clone();
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
                ValidationResultDTO validationResultDTO = await this.userService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                ValidationResultDTO validationResultDTO = await this.userService.SaveUserAsync(user);
                if (validationResultDTO.HasErrors)
                {
                    ModelState.AddModelError(string.Empty, validationResultDTO.GetErrorMessages());
                    return View("Edit", user);
                }
                return RedirectToAction("Index");
            }
            return View("Edit", user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
