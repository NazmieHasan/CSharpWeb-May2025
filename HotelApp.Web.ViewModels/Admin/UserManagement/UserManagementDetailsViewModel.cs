namespace HotelApp.Web.ViewModels.Admin.UserManagement
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;

    public class UserManagementDetailsViewModel : UserManagementIndexViewModel
    {
        public IEnumerable<BookingInfoViewModel> Bookings { get; set; } = new List<BookingInfoViewModel>();
    }
}
