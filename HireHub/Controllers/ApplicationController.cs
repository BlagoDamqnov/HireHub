namespace HireHub.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ApplicationController : UserController
    {
        public IActionResult Apply()
        {
            return View();
        }
    }
}
