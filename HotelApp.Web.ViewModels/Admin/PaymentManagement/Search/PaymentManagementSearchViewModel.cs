namespace HotelApp.Web.ViewModels.Admin.PaymentManagement.Search
{

    public class PaymentManagementSearchViewModel
    {
        public PaymentManagementSearchInputModel Search { get; set; } = new();

        public bool HasSearched { get; set; } = false;

        public IEnumerable<PaymentManagementSearchResultViewModel> Results { get; set; }
            = new List<PaymentManagementSearchResultViewModel>();
    }
}
