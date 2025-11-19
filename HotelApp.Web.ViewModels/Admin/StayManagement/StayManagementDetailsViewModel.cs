namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    public class StayManagementDetailsViewModel
    {
        public Guid Id { get; set; }
        public string GuestEmail { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime? CheckoutOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
