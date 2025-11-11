namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.UserManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class UserManagementController : BaseAdminController
    {
        private readonly IUserManagementService userService;

        public UserManagementController(IUserManagementService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementIndexViewModel> allUsers = await this.userService
                .GetUserManagementBoardDataAsync(this.GetUserId()!);

            return View(allUsers);
        }
    }
}
