namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;

    public interface IBookingManagementService
    {
        Task<IEnumerable<BookingManagementIndexViewModel>> GetBookingManagementBoardDataAsync();

        Task<BookingManagementEditFormModel?> GetBookingForEditAsync(string? id);

        Task<bool> PersistUpdatedBookingAsync(BookingManagementEditFormModel inputModel);
    }
}
