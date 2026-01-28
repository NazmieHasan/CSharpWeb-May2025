namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    using static HotelApp.GCommon.ApplicationConstants;

    public class BookingManagementReportGuestCountPdf : BookingManagementReportGuestCountSearchViewModel
    {
        public string PdfTitle => AppName;

        public string PdfOwnerUser { get; set; } = null!;

        public string PdfGeneratedOn { get; set; } = null!;
    }
}
