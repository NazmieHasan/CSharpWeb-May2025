namespace HotelApp.Web.ViewModels.Admin.GuestManagement
{
    using System.ComponentModel.DataAnnotations;

    public class GuestManagementCreateViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; } = null!;

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = ValidationMessages.Guest.GuestEmailRequiredMessage)]
        [EmailAddress(ErrorMessage = ValidationMessages.Guest.GuestEmailInvalidMessage)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateOnly BirthDate { get; set; }
    }
}
