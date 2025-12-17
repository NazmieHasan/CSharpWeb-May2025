namespace HotelApp.Web.ViewModels.Admin.CategoryManagement
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Data.Common.EntityConstants.Category;
    using static HotelApp.Web.ViewModels.ValidationMessages.Category;

    public class CategoryManagementFormInputModel
    {

        // Id does not have validation, since the model is shared between Add and Edit
        // Id will be validated in the corresponding Service method
        public int Id { get; set; }

        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequiredMessage)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;


        [Required(ErrorMessage = BedsRequiredMessage)]
        [Range(BedsMin, BedsMax, ErrorMessage = BedsRangeMessage)]
        public int Beds { get; set; }

        public decimal Price { get; set; }

        public IFormFile? Image { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}
