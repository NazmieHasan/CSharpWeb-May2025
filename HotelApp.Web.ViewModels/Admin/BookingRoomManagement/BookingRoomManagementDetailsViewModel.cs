namespace HotelApp.Web.ViewModels.Admin.BookingRoomManagement
{
    public class BookingRoomManagementDetailsViewModel : BookingRoomManagementIndexViewModel
    {
        public int AdultsCountPerRoom { get; set; }

        public int ChildCountPerRoom { get; set; }

        public int BabyCountPerRoom { get; set; }
    }
}
