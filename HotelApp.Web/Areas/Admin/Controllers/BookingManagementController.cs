namespace HotelApp.Web.Areas.Admin.Controllers
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using Microsoft.AspNetCore.Mvc;

    using Services.Core.Admin.Interfaces;

    using System.Collections.Generic;

    public class BookingManagementController : BaseAdminController
    {
        private readonly IBookingManagementService bookingService;

        public BookingManagementController(IBookingManagementService bookingService)
        {
            this.bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<BookingManagementIndexViewModel> allBookings = await this.bookingService
                .GetBookingManagementBoardDataAsync();

            return View(allBookings);
        }
    }
}
