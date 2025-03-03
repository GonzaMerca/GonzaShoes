using System.Diagnostics;
using GonzaShoes.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using GonzaShoes.Service.Interfaces;
using GonzaShoes.Model.DTOs;

namespace GonzaShoes.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            this.accountService = accountService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            LoginResultDTO resultDTO = await this.accountService.LoginUserAsync(request.Email, request.Password);

            if (resultDTO.ValidationResult.HasErrors)
                return Unauthorized(new { message = resultDTO.ValidationResult.GetErrorMessages() });

            Response.Cookies.Append("AuthToken", resultDTO.Token, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });

            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login", "Account");
        }
    }
}
