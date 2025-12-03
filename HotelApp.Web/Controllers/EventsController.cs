namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class EventsController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
