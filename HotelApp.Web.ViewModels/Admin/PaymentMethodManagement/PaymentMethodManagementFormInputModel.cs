namespace HotelApp.Web.ViewModels.Admin.PaymentMethodManagement
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Data.Common.EntityConstants.PaymentMethod;
    using static HotelApp.Web.ViewModels.ValidationMessages.PaymentMethod;

    public class PaymentMethodManagementFormInputModel
    {
        // Id does not have validation, since the model is shared between Add and Edit
        // Id will be validated in the corresponding Service method
        public int Id { get; set; }

        [Required(ErrorMessage = NameRequiredMessage)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;
    }
}
