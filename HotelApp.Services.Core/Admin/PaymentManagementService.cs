namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.PaymentManagement;
    using static HotelApp.Web.ViewModels.ValidationMessages;

    public class PaymentManagementService : IPaymentManagementService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IBookingRepository bookingRepository;

        public PaymentManagementService(IPaymentRepository paymentRepository, 
            IPaymentMethodRepository paymentMethodRepository, IBookingRepository bookingRepository)
        {
            this.paymentRepository = paymentRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.bookingRepository = bookingRepository;
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

        public async Task AddPaymentManagementAsync(PaymentManagementCreateViewModel inputModel)
        {
            if (!inputModel.Amount.HasValue)
            {
                throw new InvalidOperationException("Amount is required.");
            }

            if (!inputModel.PaymentMethodId.HasValue)
            {
                throw new InvalidOperationException("Payment method is required.");
            }

            var paymentMethodRef = await this.paymentMethodRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(pm => pm.Id == inputModel.PaymentMethodId.Value);

            if (paymentMethodRef == null)
            {
                throw new InvalidOperationException("Invalid payment method.");
            }

            var newPayment = new Payment
            {
                PaymentUserFullName = inputModel.FullName,
                PaymentUserPhoneNumber = inputModel.PhoneNumber,
                Amount = inputModel.Amount.Value,
                BookingId = inputModel.BookingId,
                PaymentMethodId = inputModel.PaymentMethodId.Value
            };

            await this.paymentRepository.AddAsync(newPayment);

            var booking = await this.bookingRepository
                .GetAllAttached()
                .Include(b => b.Room)
                    .ThenInclude(r => r.Category)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == inputModel.BookingId);

            if (booking != null)
            {
                decimal totalAmount = booking.TotalAmount;
                decimal paidAmount = booking.Payments.Sum(p => p.Amount);

                if (paidAmount >= totalAmount)
                    booking.StatusId = 3; // Fully paid

                await this.bookingRepository.SaveChangesAsync();
            }
        }

    }
}
