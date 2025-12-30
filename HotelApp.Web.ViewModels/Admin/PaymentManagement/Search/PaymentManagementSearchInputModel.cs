namespace HotelApp.Web.ViewModels.Admin.PaymentManagement.Search
{
    using System.ComponentModel.DataAnnotations;

    public class PaymentManagementSearchInputModel
    {
        public string? Id { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? BookingId { get; set; }

        public string? PaymentUserFullName { get; set; }

        public bool? IsDeleted { get; set; }


        [Display(Name = "Payment Method")]
        public int? PaymentMethodId { get; set; }

        public IEnumerable<AddPaymentMethodDropDownModel> PaymentMethods { get; set; } =
            new List<AddPaymentMethodDropDownModel>();
    }
}
