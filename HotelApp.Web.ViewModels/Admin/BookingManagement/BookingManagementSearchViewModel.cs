namespace HotelApp.Web.ViewModels.Admin.BookingManagement
{
    public class BookingManagementSearchViewModel
    {
        public BookingManagementSearchInputModel Search { get; set; } = new();

        public bool HasSearched { get; set; } = false;

        public IEnumerable<BookingManagementSearchResultViewModel> Results { get; set; }
            = new List<BookingManagementSearchResultViewModel>();
    }
}
