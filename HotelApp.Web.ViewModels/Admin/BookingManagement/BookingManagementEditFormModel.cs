namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    public class BookingManagementEditFormModel
    {
        public string Id { get; set; } = null!;

        public IEnumerable<string>? AppManagerEmails { get; set; }

        public string? ManagerEmail { get; set; }

        public DateOnly DateDeparture { get; set; }

        [Required(ErrorMessage = StatusRequiredMessage)]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public IEnumerable<AddBookingStatusDropDownModel> Statuses { get; set; } =
            new List<AddBookingStatusDropDownModel>();
    }
}
