namespace HotelApp.Web.ViewModels.Admin.RoomManagement
{
    public class RoomManagementDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int CategoryBeds { get; set; }

        public bool IsDeleted { get; set; }
    }
}
