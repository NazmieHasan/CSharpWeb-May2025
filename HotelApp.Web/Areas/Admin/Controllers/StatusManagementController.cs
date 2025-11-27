namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.StatusManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using static ViewModels.ValidationMessages.Status;
    using static GCommon.ApplicationConstants;

    public class StatusManagementController : BaseAdminController
    {
        private readonly IStatusManagementService statusService;

        public StatusManagementController(IStatusManagementService statusService)
        {
            this.statusService = statusService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<StatusManagementIndexViewModel> allStatuses = await this.statusService
                .GetStatusManagementBoardDataAsync();

            return View(allStatuses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StatusManagementFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.statusService.AddStatusManagementAsync(inputModel);

                return this.RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                this.ModelState.AddModelError(string.Empty, ServiceCreateError);
                return this.View(inputModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ToggleDelete(int? id)
        {
            Tuple<bool, bool> opResult = await this.statusService
                .DeleteOrRestoreStatusAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Status could not be found!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Status {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }

    }
}

