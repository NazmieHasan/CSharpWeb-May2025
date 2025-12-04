namespace HotelApp.Web.ViewModels.Admin.RoomManagement
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;

    public class RoomManagementDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int CategoryBeds { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<BookingInfoViewModel> Bookings { get; set; } = new List<BookingInfoViewModel>();
    }
}
