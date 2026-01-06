namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Data.Models;
    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Admin.BookingRoomManagement;

    public interface IBookingRoomManagementService
    {
        Task<BookingRoom?> FindBookingRoomByIdAsync(Guid id);

        Task<IEnumerable<BookingRoomManagementIndexViewModel>> GetBookingRoomManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize);

        Task<int> GetTotalBookingRoomsCountAsync();

        Task<BookingRoomManagementDetailsViewModel?> GetBookingRoomManagementDetailsByIdAsync(string? id);

        Task<BookingRoomManagementEditFormModel?> GetBookingRoomEditFormModelAsync(string? id);

        Task<bool> EditBookingRoomAsync(BookingRoomManagementEditFormModel? inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestoreBookingRoomAsync(string? id);
    }
}
