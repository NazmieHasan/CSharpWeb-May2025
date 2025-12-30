namespace HotelApp.Web.ViewModels.Admin.StayManagement.Search
{
    public class StayManagementSearchInputModel
    {
        public string? Id { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? BookingId { get; set; }

        public string? GuestId { get; set; }

        public DateTime? CheckoutOn { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
