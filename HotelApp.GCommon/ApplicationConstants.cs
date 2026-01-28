namespace HotelApp.GCommon
{
    public class ApplicationConstants
    {
        public const string AppName = "Hotel & SPA";
        public const string AppDateFormat = "yyyy-MM-dd";
        public const string AppDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public const string AppCurrency = "$";

        public const string IsDeletedPropertyName = "IsDeleted";

        public const string AccessDeniedPath = "/Home/AccessDenied";
        public const string ManagerAuthCookie = "ManagerAuth";
        public const string AllowAllDomainsPolicy = "AllowAllDomainsDebug";

        public const string UserRoleName = "User";
        public const string AdminRoleName = "Admin";
        public const string AdminAreaName = "Admin";

        public const string InfoMessageKey = "info";
        public const string WarningMessageKey = "warning";
        public const string ErrorMessageKey = "error";
        public const string SuccessMessageKey = "success";

        public const int MyBookingsPaginationPageSize = 5;
        public const int AdminPaginationPageSize = 10;

        public const int AllowedMaxCountRoomByCategoryForBooking = 3;
    }
}
