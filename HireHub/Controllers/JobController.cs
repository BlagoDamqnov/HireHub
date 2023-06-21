namespace HireHub.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class JobController : Controller
    {
        public IActionResult Explore()
        {
            return View();
        }
    }
}
