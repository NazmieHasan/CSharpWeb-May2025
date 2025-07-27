namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;

    public interface IBookingService
    {
        Task<IEnumerable<AllBookingsIndexViewModel>> GetAllBookingsAsync();

        Task<bool> AddBookingAsync(string userId, AddBookingInputModel inputModel);

        Task<BookingDetailsViewModel?> GetBookingDetailsByIdAsync(string? id);

        Task<EditBookingInputModel?> GetBookingForEditAsync(string? id);

        Task<bool> PersistUpdatedBookingAsync(EditBookingInputModel inputModel);

        Task<bool> SoftDeleteBookingAsync(string? id);

        Task<bool> DeleteBookingAsync(string? id);

        Task<DeleteBookingViewModel?> GetBookingDeleteDetailsByIdAsync(string? id);
    }
}
