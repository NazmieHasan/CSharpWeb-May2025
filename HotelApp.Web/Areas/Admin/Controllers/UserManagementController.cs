namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.GuestManagement;
    using HotelApp.Web.ViewModels.Admin.UserManagement;
    using Microsoft.AspNetCore.Mvc;
    using Services.Core.Admin.Interfaces;

    using static GCommon.ApplicationConstants;

    public class UserManagementController : BaseAdminController
    {
        private readonly IUserManagementService userService;

        public UserManagementController(IUserManagementService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementIndexViewModel> allUsers = await this.userService
                .GetUserManagementBoardDataAsync(this.GetUserId()!);

            return View(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleSelectionInputModel inputModel)
        {
            try
            {
                await this.userService
                    .AssignUserToRoleAsync(inputModel);
                TempData[SuccessMessageKey] = "User assigned to role successfully!";

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] = e.Message;

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(RoleSelectionInputModel inputModel)
        {
            try
            {
                bool removed = await userService.RemoveUserFromRoleAsync(inputModel.UserId, inputModel.Role);
                TempData[removed ? SuccessMessageKey : ErrorMessageKey] = removed
                    ? $"Role '{inputModel.Role}' removed successfully!"
                    : "User does not have this role.";
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] = e.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                UserManagementDetailsViewModel? userDetails = await this.userService
                    .GetUserManagementDetailsByIdAsync(id);

                if (userDetails == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(userDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }

        }

    }
}
