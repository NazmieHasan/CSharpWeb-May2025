namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
