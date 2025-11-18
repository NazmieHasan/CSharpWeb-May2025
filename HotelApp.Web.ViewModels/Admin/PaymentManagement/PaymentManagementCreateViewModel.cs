namespace HotelApp.Web.ViewModels.Admin.PaymentManagement
{
    using System.ComponentModel.DataAnnotations;

    public class PaymentManagementCreateViewModel
    {
        [Required]
        public string FullName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Guid BookingId { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }

        public IEnumerable<AddPaymentMethodDropDownModel> PaymentMethods { get; set; } =
            new List<AddPaymentMethodDropDownModel>();
    }
}
