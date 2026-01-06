namespace HotelApp.Web.ViewModels.Admin.BookingRoomManagement
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;
    using static HotelApp.Web.ViewModels.ValidationMessages.BookingRoom;

    public class BookingRoomManagementEditFormModel
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = AdultRequiredMessage)]
        [Range(1, 4, ErrorMessage = AdultsMinCountMessage)]
        [Display(Name = "Number of Adults")]
        public int AdultsCount { get; set; }

        [Display(Name = "Number of Children (age 4–17)")]
        public int ChildCount { get; set; }

        [Display(Name = "Number of Babies (age 0–3)")]
        public int BabyCount { get; set; }

        public int MaxGuests { get; set; }

        public DateOnly DateDeparture { get; set; }

        [Required(ErrorMessage = StatusRequiredMessage)]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public IEnumerable<AddBookingRoomStatusDropDownModel> Statuses { get; set; } =
            new List<AddBookingRoomStatusDropDownModel>();
    }
}
