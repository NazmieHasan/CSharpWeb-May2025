namespace HotelApp.Web.ViewModels.Admin.PaymentManagement
{
    public class PaymentManagementDetailsViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Amount { get; set; }

        public string PaymentUserFullName { get; set; } = null!;

        public string PaymentUserPhoneNumber { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public int PaymentMethodId { get; set; }

        public string PaymentMethodName { get; set; } = null!;
    }
}
