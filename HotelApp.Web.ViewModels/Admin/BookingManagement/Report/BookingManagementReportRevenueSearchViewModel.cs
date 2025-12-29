namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    public class BookingManagementReportRevenueSearchViewModel
    {
        public BookingManagementReportSearchInputModel ReportSearch { get; set; } = new();

        public bool HasReportSearched { get; set; } = false;

        public IEnumerable<BookingManagementReportRevenueSearchResultViewModel> ReportResults { get; set; }
            = new List<BookingManagementReportRevenueSearchResultViewModel>();

        public decimal TotalRevenue { get; set; }
    }
}