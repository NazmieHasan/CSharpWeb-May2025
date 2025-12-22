namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using HotelApp.Web.ViewModels.Admin.StatusManagement;

    public interface IStatusManagementService
    {
        Task<IEnumerable<StatusManagementIndexViewModel>> GetStatusManagementBoardDataAsync();

        Task<IEnumerable<AddBookingStatusDropDownModel>> GetAllowedStatusesAsync(int currentStatusId, DateOnly dateDeparture, string bookingIdString);

        Task AddStatusManagementAsync(StatusManagementFormInputModel inputModel);

        Task<IEnumerable<AddBookingStatusDropDownModel>> GetStatusesDropDownDataAsync();

        Task<Tuple<bool, bool>> DeleteOrRestoreStatusAsync(int? id);
    }
}
