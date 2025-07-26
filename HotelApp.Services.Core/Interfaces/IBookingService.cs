namespace HotelApp.Services.Core.Interfaces
{
    using HotelApp.Web.ViewModels.Booking;

    public interface IBookingService
    {
        Task<IEnumerable<AllBookingsIndexViewModel>> GetAllBookingsAsync();

        Task AddBookingAsync(AddBookingInputModel inputModel);
    }
}
