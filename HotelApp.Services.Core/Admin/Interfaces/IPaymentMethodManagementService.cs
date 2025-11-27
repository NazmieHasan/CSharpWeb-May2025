namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public interface IPaymentMethodManagementService
    {
        Task<IEnumerable<PaymentMethodManagementIndexViewModel>> GetPaymentMethodManagementBoardDataAsync();

        Task<IEnumerable<AddPaymentMethodDropDownModel>> GetPaymentMethodsDropDownDataAsync();

        Task AddPaymentMethodManagementAsync(PaymentMethodManagementFormInputModel inputModel);

        Task<PaymentMethodManagementFormInputModel?> GetEditablePaymentMethodByIdAsync(int? id);

        Task<bool> EditPaymentMethodAsync(PaymentMethodManagementFormInputModel inputModel);

        Task<Tuple<bool, bool>> DeleteOrRestorePaymentMethodAsync(int? id);
    }
}
