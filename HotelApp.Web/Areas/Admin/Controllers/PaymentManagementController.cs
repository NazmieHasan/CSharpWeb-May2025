namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    using static HotelApp.Web.ViewModels.ValidationMessages.PaymentMessages;


    public class PaymentManagementController : BaseAdminController
    {
        private readonly IPaymentManagementService paymentService;
        private readonly IPaymentMethodManagementService paymentMethodService;
        private readonly IBookingManagementService bookingService;

        public PaymentManagementController(IPaymentManagementService paymentService, 
            IPaymentMethodManagementService paymentMethodService,
            IBookingManagementService bookingService)
        {
            this.paymentService = paymentService;
            this.paymentMethodService = paymentMethodService;
            this.bookingService = bookingService;
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
            var booking = await this.bookingService.FindBookingByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            decimal paidAmount = booking.Payments.Sum(p => p.Amount);
            decimal remainingAmount = booking.TotalAmount - paidAmount;

            var model = new PaymentManagementCreateViewModel
            {
                BookingId = bookingId,
                PaymentMethods = await this.paymentMethodService.GetPaymentMethodsDropDownDataAsync(),
                RemainingAmount = remainingAmount
            };

            return PartialView("_Create", model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(PaymentManagementCreateViewModel inputModel)
        {
            var booking = await this.bookingService.FindBookingByIdAsync(inputModel.BookingId);

            if (booking == null)
            {
                return NotFound();
            }

            decimal paidAmount = booking.Payments.Sum(p => p.Amount);
            inputModel.RemainingAmount = booking.TotalAmount - paidAmount;
            inputModel.PaymentMethods = await this.paymentMethodService.GetPaymentMethodsDropDownDataAsync();

            if (!ModelState.IsValid)
            {
                return PartialView("_Create", inputModel);
            }

            if (inputModel.Amount > inputModel.RemainingAmount)
            {
                ModelState.AddModelError(nameof(inputModel.Amount),
                    ValidationMessages.PaymentMessages.AmountBookingRemainingAmountMessage);
                return PartialView("_Create", inputModel);
            }

            if (inputModel.PaymentMethodId == null || inputModel.PaymentMethodId == 0)
            {
                ModelState.AddModelError(nameof(inputModel.PaymentMethodId),
                    ValidationMessages.PaymentMessages.PaymentMethodRequiredMessage);
                return PartialView("_Create", inputModel);
            }

            try
            {
                await this.paymentService.AddPaymentManagementAsync(inputModel);

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Details", "BookingManagement", new { id = inputModel.BookingId })
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Details", "BookingManagement", new { id = inputModel.BookingId });
            }
        }

    }
}
