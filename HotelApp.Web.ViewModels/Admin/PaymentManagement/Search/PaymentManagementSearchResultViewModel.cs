namespace HotelApp.Web.ViewModels.Admin.PaymentManagement.Search
{
    public class PaymentManagementSearchResultViewModel
    {
        public string Id { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string BookingId { get; set; } = null!;
        public string PaymentUserFullName { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
