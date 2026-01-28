namespace HotelApp.Web.ViewModels.Admin.BookingManagement.Report
{
    using System.Globalization;

    public class BookingManagementReportSearchInputModel
    {
        public int Year { get; set; } = DateTime.UtcNow.Year;

        public int Month { get; set; } = DateTime.UtcNow.Month;

        public string MonthName => CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(Month);

        public string MonthYear => $"{MonthName} {Year}";
    }
}