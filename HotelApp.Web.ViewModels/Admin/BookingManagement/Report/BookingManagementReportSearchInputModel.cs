namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    public class BookingManagementReportSearchInputModel
    {
        public int Year { get; set; } = DateTime.UtcNow.Year;

        public int Month { get; set; } = DateTime.UtcNow.Month;
    }
}