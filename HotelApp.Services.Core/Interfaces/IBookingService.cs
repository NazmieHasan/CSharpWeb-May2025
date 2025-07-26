namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Booking;
    using HotelApp.Web.ViewModels.Room;

    public interface IBookingService
    {
        Task<IEnumerable<AllBookingsIndexViewModel>> GetAllBookingsAsync();

        Task AddBookingAsync(AddBookingInputModel inputModel);

        Task<BookingDetailsViewModel?> GetBookingDetailsByIdAsync(string? id);

        Task<EditBookingInputModel?> GetBookingForEditAsync(string? id);

        Task<bool> PersistUpdatedBookingAsync(EditBookingInputModel inputModel);
    }
}
