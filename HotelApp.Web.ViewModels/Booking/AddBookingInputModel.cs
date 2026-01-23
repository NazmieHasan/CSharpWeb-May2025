namespace HotelApp.Web.ViewModels.Booking
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    public class AddBookingInputModel
    {
        [Required]
        public DateOnly DateArrival { get; set; }

        [Required]
        public DateOnly DateDeparture { get; set; }

        public string? RoomCategoryName { get; set; }

        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = OwnerRequiredMessage)]
        [Display(Name = "Owner Name and Family")]
        public string Owner { get; set; } = null!;

        [Display(Name = "Booking for another person?")]
        public bool IsForAnotherPerson { get; set; }

        [MinLength(1)]
        public List<AddBookingRoomInputModel> Rooms { get; set; }
        = new();
    }
}
