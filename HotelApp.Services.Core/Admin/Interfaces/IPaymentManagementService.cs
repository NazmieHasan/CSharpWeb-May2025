namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.GCommon;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public interface IPaymentManagementService
    {
        Task<IEnumerable<PaymentManagementIndexViewModel>> GetPaymentManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize);

        Task<int> GetTotalPaymentsCountAsync();

        Task AddPaymentManagementAsync(PaymentManagementCreateViewModel inputModel);

        Task<PaymentManagementDetailsViewModel?> GetPaymentManagementDetailsByIdAsync(string? id);

        Task<Tuple<bool, bool>> DeleteOrRestorePaymentAsync(string? id);
    }
}
