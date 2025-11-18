namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;
    using HotelApp.Data.Repository;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public class PaymentMethodManagementService : IPaymentMethodManagementService
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public PaymentMethodManagementService(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<IEnumerable<PaymentMethodManagementIndexViewModel>> GetPaymentMethodManagementBoardDataAsync()
        {
            return await paymentMethodRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(p => new PaymentMethodManagementIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                })
                .ToListAsync()
                ?? Enumerable.Empty<PaymentMethodManagementIndexViewModel>();
        }

        // not added GetPaymentmethodsDropDownDataAsync() to IPaymentMethodRepository because the method
        // use a ViewModel, includes a projection
        // TO DO: Move the projection to the repository as a helper method (but not part of the public interface),
        // which returns IQueryable<PaymentMethod> or an anonymous DTO,
        // and then apply .Select(...) to a ViewModel in the service layer.
        public async Task<IEnumerable<AddPaymentMethodDropDownModel>> GetPaymentMethodsDropDownDataAsync()
        {
            IEnumerable<AddPaymentMethodDropDownModel> paymentMethodsAsDropDown = await this.paymentMethodRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(pm => new AddPaymentMethodDropDownModel()
                {
                    Id = pm.Id,
                    Name = pm.Name
                })
                .ToArrayAsync();

            return paymentMethodsAsDropDown;
        }
    }
}
