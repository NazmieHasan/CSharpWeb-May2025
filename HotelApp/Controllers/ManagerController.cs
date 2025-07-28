namespace HotelApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ManagerController : BaseController
    {
        public IActionResult Index()
        {
            return this.Ok("You are in manager index page.");
        }
    }
}
