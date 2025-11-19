namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Data.Models;
    using HotelApp.Web.ViewModels.Admin.BookingManagement;

    public interface IBookingManagementService
    {
        Task<Booking?> FindBookingByIdAsync(Guid id);

        Task<IEnumerable<BookingManagementIndexViewModel>> GetBookingManagementBoardDataAsync();

        Task<BookingManagementDetailsViewModel?> GetBookingManagementDetailsByIdAsync(string? id);

        Task<BookingManagementEditFormModel?> GetBookingEditFormModelAsync(string? id);

        Task<bool> EditBookingAsync(BookingManagementEditFormModel? inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreBookingAsync(string? id);
    }
}
