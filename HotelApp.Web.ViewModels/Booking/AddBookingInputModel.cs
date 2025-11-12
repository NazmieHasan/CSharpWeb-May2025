namespace HotelApp.Web.ViewModels.Booking
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    public class AddBookingInputModel
    {
        [Required(ErrorMessage = DateArrivalPastMessage)]
        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date")]
        public DateOnly DateArrival { get; set; }

        [Required(ErrorMessage = DateDepartureBeforeArrivalMessage)]
        [DataType(DataType.Date)]
        [Display(Name = "Departure Date")]
        public DateOnly DateDeparture { get; set; }

        [Required(ErrorMessage = AdultRequiredMessage)]
        [Range(1, 4, ErrorMessage = AdultsMinCountMessage)]
        [Display(Name = "Number of Adults")]
        public int AdultsCount { get; set; }

        [Display(Name = "Number of Children (age 4–17)")]
        public int ChildCount { get; set; }

        [Display(Name = "Number of Babies (age 0–3)")]
        public int BabyCount { get; set; }
    }
}
