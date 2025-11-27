namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.PaymentMethodManagement;
    using HotelApp.Web.ViewModels.Admin.PaymentManagement;
    using System;
    using HotelApp.Web.ViewModels.Admin.CategoryManagement;

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
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(p => new PaymentMethodManagementIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsDeleted = p.IsDeleted
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

        public async Task AddPaymentMethodManagementAsync(PaymentMethodManagementFormInputModel inputModel)
        {
            PaymentMethod newPaymentMethod = new PaymentMethod()
            {
                Name = inputModel.Name
            };

            await this.paymentMethodRepository.AddAsync(newPaymentMethod);
        }

        public async Task<PaymentMethodManagementFormInputModel?> GetEditablePaymentMethodByIdAsync(int? id)
        {
            PaymentMethodManagementFormInputModel? editablePaymentMethod = null;

            if (id.HasValue)
            {
                editablePaymentMethod = await this.paymentMethodRepository
                .GetAllAttached()
                    .AsNoTracking()
                    .Where(p => p.Id == id.Value)
                    .Select(p => new PaymentMethodManagementFormInputModel()
                    {
                        Name = p.Name
                    })
                    .SingleOrDefaultAsync();
            }

            return editablePaymentMethod;
        }

        public async Task<bool> EditPaymentMethodAsync(PaymentMethodManagementFormInputModel inputModel)
        {
            bool result = false;

            PaymentMethod? editablePaymentMethod = await this.FindPaymentMethodById(inputModel.Id);

            if (editablePaymentMethod == null)
            {
                return false;
            }

            editablePaymentMethod.Name = inputModel.Name;
            result = await this.paymentMethodRepository.UpdateAsync(editablePaymentMethod);

            return result;
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestorePaymentMethodAsync(int? id)
        {
            bool result = false;
            bool isRestored = false;
            if (id > 0)
            {
                PaymentMethod? paymentMethod = await this.paymentMethodRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(p => p.Id == id);
                if (paymentMethod != null)
                {
                    if (paymentMethod.IsDeleted)
                    {
                        isRestored = true;
                    }

                    paymentMethod.IsDeleted = !paymentMethod.IsDeleted;

                    result = await this.paymentMethodRepository
                        .UpdateAsync(paymentMethod);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

        // TODO: Implement as generic method in BaseService
        private async Task<PaymentMethod?> FindPaymentMethodById(int? id)
        {
            PaymentMethod? paymentMethod = null;

            if (id.HasValue)
            {
                paymentMethod = await this.paymentMethodRepository
                    .GetByIdAsync(id.Value);
            }

            return paymentMethod;
        }

    }
}
