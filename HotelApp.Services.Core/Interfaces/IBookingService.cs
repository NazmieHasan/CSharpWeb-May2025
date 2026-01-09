namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Manager;

    public interface IBookingService
    {
        Task<bool> AddBookingWithRoomsAsync(string userId, AddBookingInputModel inputModel);

        Task<IEnumerable<MyBookingsViewModel>> GetBookingsByUserIdAsync(string userId, int pageNumber = 1, int pageSize = ApplicationConstants.MyBookingsPaginationPageSize);

        Task<int> GetBookingsCountByUserIdAsync(string userId);

        Task<ManagerBookingDetailsViewModel?> GetBookingDetailsByIdAsync(string? id);

        Task<IEnumerable<ManagerBookingsIndexViewModel>> GetBookingsByManagerIdAsync(string userId);

        /* Booking API */
        Task<bool> AddBookingAsync(string userId, string arrival, string departure);

        /* Booking API */
        Task<IEnumerable<string>> GetBookingsIdByUserIdAsync(string? userId);
    }
}
