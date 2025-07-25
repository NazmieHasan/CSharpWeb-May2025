namespace HotelApp.Web.ViewModels.Category
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteCategoryViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }
    }
}
