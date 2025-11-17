namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    using HotelApp.Data.Models;

    public class BookingManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string UserEmail { get; set; } = null!;

        public string? ManagerEmail { get; set; }

        public bool IsDeleted { get; set; }
    }
}
