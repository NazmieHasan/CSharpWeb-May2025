namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    public class StayManagementDetailsViewModel
    {
        public Guid Id { get; set; }

        public Guid GuestId { get; set; }

        public string GuestFirstName { get; set; } = null!;

        public string GuestFamilyName { get; set; } = null!;

        public int GuestAge { get; set; }

        public string GuestEmail { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? CheckoutOn { get; set; }

        public bool IsDeleted { get; set; }

        public Guid BookingRoomId { get; set; }

        public string RoomName { get; set; }
    }
}
