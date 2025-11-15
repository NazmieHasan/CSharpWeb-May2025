namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;
    using HotelApp.Data.Repository;

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
    }
}
