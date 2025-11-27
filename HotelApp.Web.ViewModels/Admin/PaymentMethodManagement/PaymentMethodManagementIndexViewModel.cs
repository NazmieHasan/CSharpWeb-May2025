namespace HotelApp.Web.ViewModels.Admin.PaymentMethodManagement
{
    public class PaymentMethodManagementIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
