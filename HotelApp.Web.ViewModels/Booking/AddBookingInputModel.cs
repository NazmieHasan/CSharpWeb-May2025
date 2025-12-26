namespace HotelApp.Web.ViewModels.Booking
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    public class AddBookingInputModel
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public DateOnly DateArrival { get; set; }

        [Required]
        public DateOnly DateDeparture { get; set; }

        [Required(ErrorMessage = AdultRequiredMessage)]
        [Range(1, 4, ErrorMessage = AdultsMinCountMessage)]
        [Display(Name = "Number of Adults")]
        public int AdultsCount { get; set; }

        [Display(Name = "Number of Children (age 4–17)")]
        public int ChildCount { get; set; }

        [Display(Name = "Number of Babies (age 0–3)")]
        public int BabyCount { get; set; }

        public int MaxGuests { get; set; }

        public string? RoomCategoryName { get; set; }

        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = OwnerRequiredMessage)]
        [Display(Name = "Owner Name and Family")]
        public string Owner { get; set; } = null!;

        [Display(Name = "Booking for another person?")]
        public bool IsForAnotherPerson { get; set; }
    }
}
