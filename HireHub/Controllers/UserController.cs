using System.Security.Claims;

namespace HireHub.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class UserController : Controller
    {
        public string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }
    }
}