namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.PaymentManagement;
    using HotelApp.Web.ViewModels.Admin.GuestManagement;
    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.GCommon;

    using static HotelApp.Services.Core.DateTimeExtensions;

    public class PaymentManagementService : IPaymentManagementService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingManagementService bookingService;

        public PaymentManagementService(IPaymentRepository paymentRepository, 
            IPaymentMethodRepository paymentMethodRepository, 
            IBookingRepository bookingRepository,
            IBookingManagementService bookingService)
        {
            this.paymentRepository = paymentRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.bookingRepository = bookingRepository;
            this.bookingService = bookingService;
        }

        public async Task<IEnumerable<PaymentManagementIndexViewModel>> GetPaymentManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize)
        {
            var query = this.paymentRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new PaymentManagementIndexViewModel
                {
                    Id = p.Id,
                    CreatedOn = p.CreatedOn.ToHotelTime(),
                    PaymentUserFullName = p.PaymentUserFullName,
                    IsDeleted = p.IsDeleted,
                });

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalPaymentsCountAsync()
        {
            return await paymentRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .CountAsync();
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
            await this.paymentRepository.SaveChangesAsync();

            var booking = await this.bookingService.FindBookingByIdAsync(inputModel.BookingId);

            if (booking != null)
            {
                decimal totalAmount = booking.TotalAmount;
                decimal paidAmount = booking.Payments.Sum(p => p.Amount);

                // If fully paid, update status
                if (paidAmount == totalAmount)
                {
                    booking.StatusId = 3;  // For Implementation
                    await this.bookingRepository.SaveChangesAsync();
                }
            }
        }

        public async Task<PaymentManagementDetailsViewModel?> GetPaymentManagementDetailsByIdAsync(string? id)
        {
            PaymentManagementDetailsViewModel? paymentDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid paymentId);

            if (isIdValidGuid)
            {
                paymentDetails = await this.paymentRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(p => p.Id == paymentId)
                    .Select(p => new PaymentManagementDetailsViewModel()
                    {
                        Id = p.Id,
                        CreatedOn = p.CreatedOn.ToHotelTime(),
                        Amount = p.Amount,
                        PaymentUserFullName = p.PaymentUserFullName,
                        PaymentUserPhoneNumber = p.PaymentUserPhoneNumber,
                        IsDeleted = p.IsDeleted,
                        PaymentMethodName = p.PaymentMethod.Name,
                        BookingId = p.BookingId
                    })
                    .SingleOrDefaultAsync();
            }

            return paymentDetails;
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestorePaymentAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Payment? payment = await this.paymentRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(p => p.Id.ToString().ToLower() == id.ToLower());
                if (payment != null)
                {
                    if (payment.IsDeleted)
                    {
                        isRestored = true;
                    }

                    payment.IsDeleted = !payment.IsDeleted;

                    result = await this.paymentRepository
                        .UpdateAsync(payment);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

    }
}
