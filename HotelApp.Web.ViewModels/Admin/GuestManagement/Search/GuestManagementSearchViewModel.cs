namespace HotelApp.Web.ViewModels.Admin.GuestManagement.Search
{
    public class GuestManagementSearchViewModel
    {
        public GuestManagementSearchInputModel Search { get; set; } = new();

        public bool HasSearched { get; set; } = false;

        public IEnumerable<GuestManagementSearchResultViewModel> Results { get; set; }
            = new List<GuestManagementSearchResultViewModel>();
    }
}
