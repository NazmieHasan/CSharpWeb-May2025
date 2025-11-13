namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class PaymentManagementController : BaseAdminController
    {
        private readonly IPaymentManagementService paymentService;

        public PaymentManagementController(IPaymentManagementService paymentService)
        {
            this.paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PaymentManagementIndexViewModel> allPayments = await this.paymentService
                .GetPaymentManagementBoardDataAsync();

            return View(allPayments);
        }
    }
}
