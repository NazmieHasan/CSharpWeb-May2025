namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public interface IPaymentManagementService
    {
        Task<IEnumerable<PaymentManagementIndexViewModel>> GetPaymentManagementBoardDataAsync();

        Task AddPaymentManagementAsync(PaymentManagementCreateViewModel inputModel);

        Task<PaymentManagementDetailsViewModel?> GetPaymentManagementDetailsByIdAsync(string? id);

        Task<Tuple<bool, bool>> DeleteOrRestorePaymentAsync(string? id);
    }
}
