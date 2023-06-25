namespace HireHub.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ApplicationController : Controller
    {
        public IActionResult Apply()
        {
            return View();
        }
    }
}
