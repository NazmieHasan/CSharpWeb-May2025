namespace HotelApp.Web.ViewModels.Room
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    public class FindRoomInputModel
    {
        [Required(ErrorMessage = DateArrivalPastMessage)]
        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date")]
        public DateOnly DateArrival { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        [Required(ErrorMessage = DateDepartureBeforeArrivalMessage)]
        [DataType(DataType.Date)]
        [Display(Name = "Departure Date")]
        public DateOnly DateDeparture { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(2));

        public int CategoryId { get; set; }
    }
}
