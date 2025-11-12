namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    using HotelApp.Data.Models;

    public class BookingManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateOnly DateArrival { get; set; }

        public DateOnly DateDeparture { get; set; }

        public string RoomId { get; set; } = null!;

        public string Room { get; set; } = null!;

        public string? ManagerName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
