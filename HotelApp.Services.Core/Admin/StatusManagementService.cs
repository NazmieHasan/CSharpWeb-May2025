namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StatusManagement;
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using HotelApp.Web.ViewModels;


    public class StatusManagementService : IStatusManagementService
    {
        private readonly IStatusRepository statusRepository;

        public StatusManagementService(IStatusRepository statusRepository)
        {
            this.statusRepository = statusRepository;
        }

        public async Task<IEnumerable<StatusManagementIndexViewModel>> GetStatusManagementBoardDataAsync()
        {
            return await statusRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(s => new StatusManagementIndexViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsDeleted = s.IsDeleted
                })
                .ToListAsync()
                ?? Enumerable.Empty<StatusManagementIndexViewModel>();
        }

        // not added GetStatusesDropDownDataAsync() to IStatusRepository because the method
        // use a ViewModel, includes a projection
        // TO DO: Move the projection to the repository as a helper method (but not part of the public interface),
        // which returns IQueryable<Status> or an anonymous DTO,
        // and then apply .Select(...) to a ViewModel in the service layer.
        public async Task<IEnumerable<AddBookingStatusDropDownModel>> GetStatusesDropDownDataAsync()
        {
            IEnumerable<AddBookingStatusDropDownModel> statusesAsDropDown = await this.statusRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(s => new AddBookingStatusDropDownModel()
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToArrayAsync();

            return statusesAsDropDown;
        }

        public async Task AddStatusManagementAsync(StatusManagementFormInputModel inputModel)
        {
            var existingStatus = await this.statusRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name.ToLower() == inputModel.Name.ToLower());

            if (existingStatus != null)
            {
                throw new InvalidOperationException(ValidationMessages.Status.NameAlreadyExistsMessage);
            }

            Status newStatus = new Status()
            {
                Name = inputModel.Name
            };

            await this.statusRepository.AddAsync(newStatus);
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreStatusAsync(int? id)
        {
            bool result = false;
            bool isRestored = false;
            if (id > 0)
            {
                Status? status = await this.statusRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(s => s.Id == id);
                if (status != null)
                {
                    if (status.IsDeleted)
                    {
                        isRestored = true;
                    }

                    status.IsDeleted = !status.IsDeleted;

                    result = await this.statusRepository
                        .UpdateAsync(status);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

    }
}

