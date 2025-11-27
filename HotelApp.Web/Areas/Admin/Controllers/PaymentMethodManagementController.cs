namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using static ViewModels.ValidationMessages.PaymentMethod;
    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels.Admin.CategoryManagement;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public class PaymentMethodManagementController : BaseAdminController
    {
        private readonly IPaymentMethodManagementService paymentMethodService;

        public PaymentMethodManagementController(IPaymentMethodManagementService paymentMethodService)
        {
            this.paymentMethodService = paymentMethodService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PaymentMethodManagementIndexViewModel> allPaymentMethods = await this.paymentMethodService
                .GetPaymentMethodManagementBoardDataAsync();

            return View(allPaymentMethods);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentMethodManagementFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                await this.paymentMethodService.AddPaymentMethodManagementAsync(inputModel);

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
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                PaymentMethodManagementFormInputModel? editablePaymentMethod = await this.paymentMethodService
                    .GetEditablePaymentMethodByIdAsync(id);
                if (editablePaymentMethod == null)
                {
                    return this.RedirectToAction(nameof(Index));
                }

                return this.View(editablePaymentMethod);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return this.RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PaymentMethodManagementFormInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                bool editSuccess = await this.paymentMethodService.EditPaymentMethodAsync(inputModel);
                if (!editSuccess)
                {
                    return this.RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> ToggleDelete(int? id)
        {
            Tuple<bool, bool> opResult = await this.paymentMethodService
                .DeleteOrRestorePaymentMethodAsync(id);
            bool success = opResult.Item1;
            bool isRestored = opResult.Item2;

            if (!success)
            {
                TempData[ErrorMessageKey] = "Payment method could not be found!";
            }
            else
            {
                string operation = isRestored ? "restored" : "deleted";

                TempData[SuccessMessageKey] = $"Payment method {operation} successfully!";
            }

            return this.RedirectToAction(nameof(Index));
        }


    }
}
