namespace HotelApp.Web.ViewModels.Admin.ManagerManagement
{
    using HotelApp.Data.Models;

    public class ManagerManagementDetailsViewModel 
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public string Email { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public ICollection<Booking> ManagedBookings { get; set; }
            = new HashSet<Booking>();
    }
}
