namespace HotelApp.Web.ViewModels.Booking
{
    public class RoomInfoInMyBookingViewModel
    {
        public string CategoryName { get; set; } = null!;

        public int AdultsCountPerRoom { get; set; }

        public int ChildCountPerRoom { get; set; }

        public int BabyCountPerRoom { get; set; }

        public string RoomStatus { get; set; } = null!;
    }
}
