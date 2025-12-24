namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    public class StayManagementIndexPageViewModel
    {
        public IEnumerable<StayManagementIndexViewModel> Stays { get; set; } = Enumerable.Empty<StayManagementIndexViewModel>();

        public GuestAgeStatsViewModel GuestAgeStats { get; set; } = new GuestAgeStatsViewModel();
    }
}
