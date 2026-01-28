namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    using static HotelApp.GCommon.ApplicationConstants;

    public class BookingManagementReportGuestCountPdf : BookingManagementReportGuestCountSearchViewModel
    {
        public BookingManagementReportPdf Pdf { get; set; } = new();
    }
}
