namespace HotelApp.Web.ViewModels.Category
{
    public class AllCategoriesIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int Beds { get; set; }

        public string Description { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
