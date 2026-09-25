using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionCitas.API.Controllers
{
    public abstract class CustomBaseController : ControllerBase
    {
        public string GetUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId ?? string.Empty;
        }
    }
}