namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public interface IPaymentMethodManagementService
    {
        Task<IEnumerable<PaymentMethodManagementIndexViewModel>> GetPaymentMethodManagementBoardDataAsync();

        Task<IEnumerable<AddPaymentMethodDropDownModel>> GetPaymentMethodsDropDownDataAsync();
    }
}
