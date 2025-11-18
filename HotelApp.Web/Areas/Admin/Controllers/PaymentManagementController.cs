namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class PaymentManagementController : BaseAdminController
    {
        private readonly IPaymentManagementService paymentService;
        private readonly IPaymentMethodManagementService paymentMethodService;

        public PaymentManagementController(IPaymentManagementService paymentService, IPaymentMethodManagementService paymentMethodService)
        {
            this.paymentService = paymentService;
            this.paymentMethodService = paymentMethodService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PaymentManagementIndexViewModel> allPayments = await this.paymentService
                .GetPaymentManagementBoardDataAsync();

            return View(allPayments);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid bookingId)
        {
            var model = new PaymentManagementCreateViewModel
            {
                BookingId = bookingId,
                PaymentMethods = await this.paymentMethodService.GetPaymentMethodsDropDownDataAsync(),
            };

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PaymentManagementCreateViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                // TODO: show error message
                return this.RedirectToAction("Details", "BookingManagement", new { id = inputModel.BookingId });
            }

            try
            {
                await this.paymentService.AddPaymentManagementAsync(inputModel);

                return this.RedirectToAction("Details", "BookingManagement", new { id = inputModel.BookingId });
            }
            catch (Exception e)
            {
                // TODO: Implement it with the ILogger
                Console.WriteLine(e.Message);
                return this.RedirectToAction("Details", "BookingManagement", new { id = inputModel.BookingId });
            }
        }
    }
}
