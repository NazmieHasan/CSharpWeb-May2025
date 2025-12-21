namespace HotelApp.Web.ViewModels.Room
{
    public class RoomDetailsViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public decimal CategoryPrice { get; set; }

        public int CategoryBeds { get; set; }
    }
}
