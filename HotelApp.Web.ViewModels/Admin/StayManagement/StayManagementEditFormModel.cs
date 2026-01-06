namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    using System.ComponentModel.DataAnnotations;

    public class StayManagementEditFormModel
    {
        public string Id { get; set; } = null!;

        [Required]
        public Guid BookingRoomId { get; set; }

        public DateTime? CheckoutOn { get; set; }
    }
}
