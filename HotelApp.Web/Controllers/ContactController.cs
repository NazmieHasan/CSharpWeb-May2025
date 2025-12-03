namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ContactController : BaseController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
