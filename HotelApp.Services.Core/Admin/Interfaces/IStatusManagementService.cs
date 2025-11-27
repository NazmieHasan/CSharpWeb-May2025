namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using HotelApp.Web.ViewModels.Admin.StatusManagement;

    public interface IStatusManagementService
    {
        Task<IEnumerable<StatusManagementIndexViewModel>> GetStatusManagementBoardDataAsync();

        Task AddStatusManagementAsync(StatusManagementFormInputModel inputModel);

        Task<IEnumerable<AddBookingStatusDropDownModel>> GetStatusesDropDownDataAsync();

        Task<Tuple<bool, bool>> DeleteOrRestoreStatusAsync(int? id);
    }
}
