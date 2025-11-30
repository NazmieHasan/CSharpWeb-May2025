namespace HotelApp.Web.ViewModels.Admin.UserManagement
{
    public class UserManagementDetailsViewModel : UserManagementIndexViewModel
    {
        public IEnumerable<UserBookingViewModel> Bookings { get; set; } = new List<UserBookingViewModel>();
    }
}
