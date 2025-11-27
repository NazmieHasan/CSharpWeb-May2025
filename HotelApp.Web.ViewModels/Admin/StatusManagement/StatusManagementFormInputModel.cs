namespace HotelApp.Web.ViewModels.Admin.StatusManagement
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Data.Common.EntityConstants.Status;
    using static HotelApp.Web.ViewModels.ValidationMessages.Status;

    public class StatusManagementFormInputModel
    {
        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;
    }
}
