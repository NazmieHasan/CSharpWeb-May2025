namespace HotelApp.Web.ViewModels.Admin.GuestManagement
{
    using System.ComponentModel.DataAnnotations;

    public class GuestManagementCreateViewModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string FamilyName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;
    }
}
