namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    using System.ComponentModel.DataAnnotations;

    public class StayManagementCreateViewModel
    {
        [Required]
        public Guid BookingRoomId { get; set; }

        [Required(ErrorMessage = ValidationMessages.Stay.GuestEmailRequiredMessage)]
        [EmailAddress(ErrorMessage = ValidationMessages.Stay.GuestEmailInvalidMessage)]
        public string? GuestEmail { get; set; }
    }
}
