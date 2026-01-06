namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    public class RoomInfoInBookingManagementViewModel
    {
        public string RoomId { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public string RoomCategory { get; set; } = null!;
        public int AdultsCountPerRoom { get; set; }
        public int ChildCountPerRoom { get; set; }
        public int BabyCountPerRoom { get; set; }
        public string RoomStatus { get; set; } = null!;
        public bool IsAllowedAddStay { get; set; }
        public Guid BookingRoomId { get; set; }
    }
}
