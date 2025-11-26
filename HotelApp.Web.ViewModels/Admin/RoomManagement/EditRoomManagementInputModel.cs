namespace HotelApp.Web.ViewModels.Admin.RoomManagement
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Data.Common.EntityConstants.Room;
    using static HotelApp.Web.ViewModels.ValidationMessages.Room;

    public class EditRoomManagementInputModel
    {
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = CategoryRequiredMessage)]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<AddRoomCategoryDropDownModel> Categories { get; set; } =
            new List<AddRoomCategoryDropDownModel>();
    }
}
