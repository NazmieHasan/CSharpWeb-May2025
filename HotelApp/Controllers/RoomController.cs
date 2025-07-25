namespace HotelApp.Web.Controllers
{
    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels.Room;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class RoomController : Controller
    {
        private readonly IRoomService roomService;
        private readonly ICategoryService categoryService;

        public RoomController(IRoomService roomService,
            ICategoryService categoryService)
        {
            this.roomService = roomService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<AllRoomsIndexViewModel> allRooms = await
                    this.roomService.GetAllRoomsAsync();

                return View(allRooms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }
        }
    }
}
