namespace HotelApp.Web.ViewModels.Admin.CategoryManagement
{

    public class CategoryManagementDetailsViewModel : CategoryManagementIndexViewModel
    {
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;
    }

}