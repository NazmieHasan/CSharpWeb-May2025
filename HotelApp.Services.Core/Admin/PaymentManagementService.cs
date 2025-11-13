namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.PaymentManagement;

    public class PaymentManagementService : IPaymentManagementService
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentManagementService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<IEnumerable<PaymentManagementIndexViewModel>> GetPaymentManagementBoardDataAsync()
        {
            return await paymentRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(p => new PaymentManagementIndexViewModel
                {
                    Id = p.Id,
                    CreatedOn = p.CreatedOn,
                    PaymentUserFullName = p.PaymentUserFullName,
                })
                .ToListAsync()
                ?? Enumerable.Empty<PaymentManagementIndexViewModel>();
        }
    }
}
