namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    public class BookingManagementReportGuestCountSearchViewModel
    {
        public BookingManagementReportSearchInputModel ReportSearch { get; set; } = new();

        public bool HasReportSearched { get; set; } = false;

        public IEnumerable<BookingManagementReportGuestCountSearchResultViewModel> ReportResults { get; set; }
            = new List<BookingManagementReportGuestCountSearchResultViewModel>();
    }
}