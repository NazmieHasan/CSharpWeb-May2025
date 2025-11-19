namespace HotelApp.Web.ViewModels.Admin.PaymentManagement
{
    using System.ComponentModel.DataAnnotations;

    using static HotelApp.Web.ViewModels.ValidationMessages.PaymentMessages;

    public class PaymentManagementCreateViewModel
    {
        [Required(ErrorMessage = ValidationMessages.PaymentMessages.FullNameRequiredMessage)]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = ValidationMessages.PaymentMessages.PhoneNumberRequiredMessage)]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = ValidationMessages.PaymentMessages.AmountRequiredMessage)]
        public decimal? Amount { get; set; }

        public decimal RemainingAmount { get; set; }

        [Required]
        public Guid BookingId { get; set; }

        [Required(ErrorMessage = ValidationMessages.PaymentMessages.PaymentMethodRequiredMessage)]
        [Display(Name = "Payment Method")]
        public int? PaymentMethodId { get; set; }

        public IEnumerable<AddPaymentMethodDropDownModel> PaymentMethods { get; set; } =
            new List<AddPaymentMethodDropDownModel>();
    }
}