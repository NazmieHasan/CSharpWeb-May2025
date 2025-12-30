namespace HotelApp.Web.ViewModels.Admin.StayManagement.Search
{
    public class StayManagementSearchResultViewModel
    {
        public string Id { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string BookingId { get; set; } = null!;
        public string CheckoutOn { get; set; } = "-";
        public bool IsDeleted { get; set; }
    }
}
