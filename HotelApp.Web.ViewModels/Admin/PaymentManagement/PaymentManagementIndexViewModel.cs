namespace HotelApp.Web.ViewModels.Admin.PaymentManagement
{
    public class PaymentManagementIndexViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string PaymentUserFullName { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
