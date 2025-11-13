namespace HotelApp.Services.Core.Admin.Interfaces
{
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public interface IPaymentManagementService
    {
        Task<IEnumerable<PaymentManagementIndexViewModel>> GetPaymentManagementBoardDataAsync();
    }
}
