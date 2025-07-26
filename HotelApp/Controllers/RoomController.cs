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

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                AddRoomInputModel inputModel = new AddRoomInputModel()
                {
                    Categories = await this.categoryService.GetCategoriesDropDownDataAsync(),
                };

                return this.View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRoomInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                bool addResult = await this.roomService
                    .AddRoomAsync(inputModel);

                if (addResult == false)
                {
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                RoomDetailsViewModel? roomDetails = await this.roomService
                    .GetRoomDetailsByIdAsync(id);

                if (roomDetails == null)
                {
                    return this.RedirectToAction(nameof(Index), "Home");
                }

                return this.View(roomDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index), "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                EditRoomInputModel? editInputModel = await this.roomService
                    .GetRoomForEditAsync(id);

                if (editInputModel == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                editInputModel.Categories = await this.categoryService.GetCategoriesDropDownDataAsync();

                return this.View(editInputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditRoomInputModel inputModel)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }

                bool editResult = await this.roomService
                    .PersistUpdatedRoomAsync(inputModel);

                if (editResult == false)
                {
                    this.ModelState.AddModelError(string.Empty, "Edit error");
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Details), new { id = inputModel.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

    }
}
