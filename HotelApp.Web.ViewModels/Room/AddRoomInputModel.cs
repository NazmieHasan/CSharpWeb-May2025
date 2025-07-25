namespace HotelApp.Web.ViewModels.Room
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Data.Common.EntityConstants.Room;
    using static HotelApp.Web.ViewModels.ValidationMessages.Room;

    public class AddRoomInputModel
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = CategoryRequiredMessage)]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<AddRoomCategoryDropDownModel> Categories { get; set; } =
            new List<AddRoomCategoryDropDownModel>();
    }
}
