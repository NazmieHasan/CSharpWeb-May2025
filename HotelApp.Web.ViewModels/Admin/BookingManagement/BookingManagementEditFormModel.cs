namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.Booking;

    public class BookingManagementEditFormModel
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

        public IEnumerable<string>? AppManagerEmails { get; set; }

        public string? ManagerEmail { get; set; }

        [Required(ErrorMessage = StatusRequiredMessage)]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        public IEnumerable<AddBookingStatusDropDownModel> Statuses { get; set; } =
            new List<AddBookingStatusDropDownModel>();
    }
}
