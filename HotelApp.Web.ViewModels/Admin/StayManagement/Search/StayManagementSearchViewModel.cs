namespace HotelApp.Web.ViewModels.Admin.StayManagement.Search
{
    public class StayManagementSearchViewModel
    {
        public StayManagementSearchInputModel Search { get; set; } = new();

        public bool HasSearched { get; set; } = false;

        public IEnumerable<StayManagementSearchResultViewModel> Results { get; set; }
            = new List<StayManagementSearchResultViewModel>();
    }
}
