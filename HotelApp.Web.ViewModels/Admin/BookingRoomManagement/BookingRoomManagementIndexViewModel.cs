namespace HotelApp.Web.ViewModels.Admin.BookingRoomManagement
{
    public class BookingRoomManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public Guid BookingId { get; set; }   

        public Guid RoomId { get; set; }     

        public string Status { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
