namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;

    public interface IPaymentMethodManagementService
    {
        Task<IEnumerable<PaymentMethodManagementIndexViewModel>> GetPaymentMethodManagementBoardDataAsync();
    }
}
