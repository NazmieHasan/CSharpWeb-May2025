namespace HotelApp.Web.Controllers
{
    using System.Diagnostics;
    using HotelApp.Services.Core.Interfaces;
    using HotelApp.Web.ViewModels;
    using HotelApp.Web.ViewModels.Room;
    using HotelApp.Web.ViewModels.Category;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels.Booking;

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryService categoryService;
        private readonly IRoomService roomService;

        public HomeController(ILogger<HomeController> logger,
            ICategoryService categoryService,
            IRoomService roomService)
        {
            _logger = logger;
            this.categoryService = categoryService;
            this.roomService = roomService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AllCategoriesIndexViewModel> allCategories = await this.categoryService
                .GetAllCategoriesAsync();

            return View(allCategories);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FindRoom()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> FindRoom(FindRoomInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form!";
                return RedirectToAction(nameof(Index));
            }

            if (inputModel.DateArrival < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                TempData["ErrorMessage"] = $"Arrival date {inputModel.DateArrival} cannot be in the past!";
                return RedirectToAction(nameof(Index));
            }

            if (inputModel.DateDeparture <= inputModel.DateArrival)
            {
                TempData["ErrorMessage"] = $"Departure date {inputModel.DateArrival} must be after arrival date!";
                return RedirectToAction(nameof(Index));
            }

            var rooms = await this.roomService.FindRoomByDateArrivaleAndDateDepartureAsync(inputModel);

            var resultModel = new FindRoomResultViewModel
            {
                DateArrival = inputModel.DateArrival,
                DateDeparture = inputModel.DateDeparture,
                Rooms = rooms.ToList()
            };

            return View("FindRoom", resultModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FindRoomByCategory(int categoryId)
        {
            var model = new FindRoomInputModel
            {
                CategoryId = categoryId
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> FindRoomByCategory(FindRoomInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form!";
                return View(inputModel);
            }

            if (inputModel.DateArrival < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                TempData["ErrorMessage"] = $"Arrival date cannot be in the past!";
                return View(inputModel);
            }

            if (inputModel.DateDeparture <= inputModel.DateArrival)
            {
                TempData["ErrorMessage"] = $"Departure date must be after arrival date!";
                return View(inputModel);
            }

            var room = await this.roomService
                .FindRoomByDateArrivaleDateDepartureAndCategoryAsync(inputModel);

            string categoryName = await this.categoryService
                .FindCategoryNameByCategoryId(inputModel.CategoryId);

            var resultModel = new FindRoomResultViewModel
            {
                DateArrival = inputModel.DateArrival,
                DateDeparture = inputModel.DateDeparture,
                CategoryName = categoryName,
                Rooms = room != null ? new List<AllRoomsIndexViewModel> { room } : new List<AllRoomsIndexViewModel>()
            };

            return View("FindRoom", resultModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode)
        { 
            switch (statusCode)
            {
                case 401:
                case 403:
                    return this.View("UnauthorizedError");
                case 404:
                    return this.View("NotFoundError");
                default:
                    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
