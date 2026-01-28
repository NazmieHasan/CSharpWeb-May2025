namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    using static HotelApp.GCommon.ApplicationConstants;

    public class BookingManagementReportRevenuePdf : BookingManagementReportRevenueSearchViewModel
    {
        public BookingManagementReportPdf Pdf { get; set; } = new();
    }
}
