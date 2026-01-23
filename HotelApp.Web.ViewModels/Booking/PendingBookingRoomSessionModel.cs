namespace HotelApp.Web.ViewModels.Booking
{
    public class PendingBookingRoomSessionModel
    {
        public string RoomId { get; set; } = null!;
        public int AdultsCount { get; set; }
        public int ChildCount { get; set; }
        public int BabyCount { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal CategoryPrice { get; set; }
    }
}
