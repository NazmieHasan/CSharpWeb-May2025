namespace HotelApp.Web.ViewModels.Room
{
    public class AllRoomsIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int CategoryBeds { get; set; }
    }
}
