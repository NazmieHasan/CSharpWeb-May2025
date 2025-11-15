namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

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
    }
}
