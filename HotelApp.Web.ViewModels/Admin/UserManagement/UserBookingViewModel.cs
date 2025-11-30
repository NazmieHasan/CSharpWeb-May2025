namespace HotelApp.Web.ViewModels.Admin.UserManagement
{
    public class UserBookingViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; set; } = null!;
    }
}
