namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    using static HotelApp.GCommon.ApplicationConstants;

    public class BookingManagementReportRevenueSearchResultViewModel
    {
        public string Id { get; set; } = null!;
        public DateTime CreatedOnOriginalFormat { get; set; }
        public DateOnly DateArrivalOriginalFormat { get; set; }
        public DateOnly DateDepartureOriginalFormat { get; set; }
        public string Status { get; set; } = null!;
        public decimal PaidAmount { get; set; }
        public string PaidCurrency => AppCurrency;

        public string CreatedOn => CreatedOnOriginalFormat.ToString(AppDateFormat);
        public string DateArrival => DateArrivalOriginalFormat.ToString(AppDateFormat);
        public string DateDeparture => DateDepartureOriginalFormat.ToString(AppDateFormat);
    }
}
