namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.PaymentManagement;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement.Search;
    using HotelApp.GCommon;

    using HotelApp.Services.Common.Extensions;

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

        public async Task<IEnumerable<PaymentManagementSearchResultViewModel>> SearchPaymentAsync(PaymentManagementSearchInputModel inputModel)
        {
            var query = paymentRepository
                .GetAllAttached()
                .Include(p => p.PaymentMethod)
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AsQueryable();

            // Payment Id
            if (!string.IsNullOrWhiteSpace(inputModel.Id))
            {
                if (!Guid.TryParse(inputModel.Id, out Guid paymentId))
                {
                    return new List<PaymentManagementSearchResultViewModel>();
                }
                query = query.Where(p => p.Id == paymentId);
            }

            // Created On
            if (inputModel.CreatedOn.HasValue)
            {
                var createdDate = inputModel.CreatedOn.Value.Date;

                query = query.Where(p =>
                    p.CreatedOn.Date == createdDate);
            }

            // Booking Id
            if (!string.IsNullOrWhiteSpace(inputModel.BookingId))
            {
                if (!Guid.TryParse(inputModel.BookingId, out Guid bookingId))
                {
                    return new List<PaymentManagementSearchResultViewModel>();
                }
                query = query.Where(p => p.BookingId == bookingId);
            }

            // Payment User Full Name
            if (!string.IsNullOrWhiteSpace(inputModel.PaymentUserFullName))
            {
                query = query.Where(p =>
                    p.PaymentUserFullName == inputModel.PaymentUserFullName);
            }

            // Payment Method
            if (inputModel.PaymentMethodId.HasValue)
            {
                query = query.Where(p =>
                    p.PaymentMethodId == inputModel.PaymentMethodId.Value);
            }

            // IsDeleted
            if (inputModel.IsDeleted.HasValue)
            {
                query = query.Where(p =>
                    p.IsDeleted == inputModel.IsDeleted.Value);
            }

            var payments = await query
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new PaymentManagementSearchResultViewModel
                {
                    Id = p.Id.ToString(),
                    CreatedOn = p.CreatedOn,
                    BookingId = p.BookingId.ToString(),
                    PaymentUserFullName = p.PaymentUserFullName,
                    PaymentMethod = p.PaymentMethod.Name,
                    IsDeleted = p.IsDeleted
                })
                .ToListAsync();

            return payments;
        }

    }
}
