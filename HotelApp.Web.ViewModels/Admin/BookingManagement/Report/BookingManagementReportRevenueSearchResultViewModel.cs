namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    using static HotelApp.GCommon.ApplicationConstants;

    public class BookingManagementReportRevenueSearchResultViewModel
    {
        public string Id { get; set; } = null!;
        public string CreatedOn { get; set; } = null!;
        public string DateArrival { get; set; } = null!;
        public string DateDeparture { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal PaidAmount { get; set; }
        public string PaidCurrency => AppCurrency;
    }
}
