namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Manager;

    public interface IBookingService
    {
        Task<bool> AddBookingAsync(string userId, AddBookingInputModel inputModel);

        Task<IEnumerable<MyBookingsViewModel>> GetBookingsByUserIdAsync(string userId);

        Task<ManagerBookingDetailsViewModel?> GetBookingDetailsByIdAsync(string? id);

        Task<IEnumerable<ManagerBookingsIndexViewModel>> GetBookingsByManagerIdAsync(string userId);

        /* Booking API */
        Task<bool> AddBookingAsync(string userId, string arrival, string departure, int adultsCount, int childCount, int babyCount);

        /* Booking API */
        Task<IEnumerable<string>> GetBookingsIdByUserIdAsync(string? userId);
    }
}
