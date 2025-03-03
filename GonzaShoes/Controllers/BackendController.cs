using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GonzaShoes.Controllers
{
    [Authorize]
    public class BackendController : Controller
    {
        protected int userId;

        protected void SetUser()
        {
            // Aquí extraemos el UserId del claim 'UserId' o cualquier otro claim adecuado
            var userIdClaim = HttpContext.User.FindFirst("UserId")?.Value;
            userId = userIdClaim != null ? int.Parse(userIdClaim) : 1; // Devolver 1 si no se encuentra el claim, por defecto.
        }
    }
}
