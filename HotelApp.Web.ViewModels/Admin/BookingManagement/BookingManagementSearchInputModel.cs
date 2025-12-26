namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    using System.ComponentModel.DataAnnotations;

    public class BookingManagementSearchInputModel
    {
        public string? Id { get; set; }

        public string? Owner { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateOnly? DateArrival { get; set; }

        public DateOnly? DateDeparture { get; set; }

        public bool? IsDeleted { get; set; }


        [Display(Name = "Status")]
        public int? StatusId { get; set; }

        public IEnumerable<AddBookingStatusDropDownModel> Statuses { get; set; }
            = new List<AddBookingStatusDropDownModel>();
    }
}