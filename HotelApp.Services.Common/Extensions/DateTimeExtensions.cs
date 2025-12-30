namespace HotelApp.Services.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly TimeZoneInfo HotelTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Europe/Sofia");

        public static DateTime ToHotelTime(this DateTime utc)
            => TimeZoneInfo.ConvertTimeFromUtc(utc, HotelTimeZone);

        public static DateTime? ToHotelTime(this DateTime? utc)
            => utc.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(utc.Value, HotelTimeZone) : null;
    }
}