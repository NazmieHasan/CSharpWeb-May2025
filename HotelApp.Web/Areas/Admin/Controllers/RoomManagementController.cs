namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.RoomManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class RoomManagementController : BaseAdminController
    {
        private readonly IRoomManagementService roomService;

        public RoomManagementController(IRoomManagementService roomService)
        {
            this.roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<RoomManagementIndexViewModel> allRooms = await this.roomService
                .GetRoomManagementBoardDataAsync();

            return View(allRooms);
        }
    }
}
