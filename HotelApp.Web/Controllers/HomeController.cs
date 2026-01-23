namespace HotelApp.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Interfaces;
    using Web.ViewModels;
    using Web.ViewModels.Room;
    using Web.ViewModels.Category;

    using static GCommon.ApplicationConstants;
    using static Web.ViewModels.ValidationMessages;

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
        public async Task<IActionResult> FindRoom(FindRoomInputModel inputModel)
        {
            if (Request.Query.ContainsKey("DateArrival") && Request.Query.ContainsKey("DateDeparture"))
            {
                HttpContext.Session.Remove("PendingRooms");
            }

            if (!Request.Query.ContainsKey("DateArrival") || !Request.Query.ContainsKey("DateDeparture"))
            {
                return View();
            }

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

            var daysBooked = (inputModel.DateDeparture.DayNumber - inputModel.DateArrival.DayNumber);
            if (daysBooked > 100)
            {
                TempData["ErrorMessage"] = Booking.AllowedMaxDaysCount;
                return RedirectToAction(nameof(Index));
            }

            var rooms = await this.roomService.FindRoomByDateArrivaleAndDateDepartureAsync(inputModel);

            var resultModel = new FindRoomResultViewModel
            {
                DateArrival = inputModel.DateArrival,
                DateDeparture = inputModel.DateDeparture,
                Rooms = rooms.ToList()
            };

            return View("FindRoomResult", resultModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult FindRoomByCategory(int categoryId)
        {
            if (categoryId == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new FindRoomInputModel
            {
                CategoryId = categoryId
            };

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FindRoomByCategorySearch(FindRoomInputModel inputModel)
        {
            if (Request.Query.ContainsKey("DateArrival") && Request.Query.ContainsKey("DateDeparture"))
            {
                HttpContext.Session.Remove("PendingRooms");
            }

            if (inputModel.CategoryId == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!Request.Query.ContainsKey("DateArrival") || !Request.Query.ContainsKey("DateDeparture"))
            {
                return View("FindRoomByCategory", inputModel);
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form!";
                return View("FindRoomByCategory", inputModel);
            }

            if (inputModel.DateArrival < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                TempData["ErrorMessage"] = $"Arrival date cannot be in the past!";
                return View("FindRoomByCategory", inputModel);
            }

            if (inputModel.DateDeparture <= inputModel.DateArrival)
            {
                TempData["ErrorMessage"] = $"Departure date must be after arrival date!";
                return View("FindRoomByCategory", inputModel);
            }

            var daysBooked = (inputModel.DateDeparture.DayNumber - inputModel.DateArrival.DayNumber);
            if (daysBooked > 100)
            {
                TempData["ErrorMessage"] = Booking.AllowedMaxDaysCount;
                return View("FindRoomByCategory", inputModel);
            }

            var rooms = await this.roomService
                .FindRoomByDateArrivaleDateDepartureAndCategoryAsync(inputModel);

            string categoryName = await this.categoryService
                .FindCategoryNameByCategoryId(inputModel.CategoryId);

            var resultModel = new FindRoomResultViewModel
            {
                DateArrival = inputModel.DateArrival,
                DateDeparture = inputModel.DateDeparture,
                CategoryName = categoryName,
                Rooms = rooms.ToList()
            };

            return View("FindRoomResult", resultModel);
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
