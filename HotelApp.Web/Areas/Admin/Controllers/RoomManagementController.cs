namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.GCommon;
    using HotelApp.Services.Core;
    using HotelApp.Web.ViewModels;
    using HotelApp.Web.ViewModels.Admin.RoomManagement;

    using Microsoft.AspNetCore.Mvc;
    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    using static GCommon.ApplicationConstants;

    public class RoomManagementController : BaseAdminController
    {
        private readonly IRoomManagementService roomService;
        private readonly ICategoryManagementService categoryService;

        public RoomManagementController(IRoomManagementService roomService, 
            ICategoryManagementService categoryService)
        {
            this.roomService = roomService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            try
            {
                int pageSize = ApplicationConstants.AdminPaginationPageSize;

                var pagedRooms = await roomService
                .GetRoomManagementBoardDataAsync(pageNumber, pageSize);

                int totalRooms = await roomService.GetTotalRoomsCountAsync();

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageSize = pageSize;
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalRooms / pageSize);

                return View(pagedRooms);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return this.RedirectToAction(nameof(Index), "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                AddRoomManagementInputModel inputModel = new AddRoomManagementInputModel()
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
        public async Task<IActionResult> Create(AddRoomManagementInputModel inputModel)
        {
            try
            {
                inputModel.Categories = await this.categoryService.GetCategoriesDropDownDataAsync();

                if (!this.ModelState.IsValid)
                {
                    return this.View(inputModel);
                }
                
                bool addResult = await this.roomService
                    .AddRoomManagementAsync(inputModel);

                if (addResult == false)
                {
                    return this.View(inputModel);
                }

                return this.RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                this.ModelState.AddModelError(nameof(inputModel.Name), ex.Message);
                return this.View(inputModel);
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
                RoomManagementDetailsViewModel? roomDetails = await this.roomService
                    .GetRoomDetailsByIdAsync(id);

                if (roomDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(roomDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            try
            {
                EditRoomManagementInputModel? editInputModel = await this.roomService
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
        public async Task<IActionResult> Edit(EditRoomManagementInputModel inputModel)
        {
            try
            {
                inputModel.Categories = await this.categoryService.GetCategoriesDropDownDataAsync();

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
            catch (InvalidOperationException ex)
            {
                if (ex.Message == ValidationMessages.Room.CategoryRequiredMessage ||
                    ex.Message == ValidationMessages.Room.CategoryCannotBeChangedMessage)
                {
                    ModelState.AddModelError(nameof(inputModel.CategoryId), ex.Message);
                }
                else if (ex.Message == ValidationMessages.Room.NameAlreadyExistsMessage)
                {
                    ModelState.AddModelError(nameof(inputModel.Name), ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(inputModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(string? id)
        {
            Tuple<bool, bool> opResult = await this.roomService
                .DeleteOrRestoreRoomAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Room could not be found!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Room {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }

    }
}
