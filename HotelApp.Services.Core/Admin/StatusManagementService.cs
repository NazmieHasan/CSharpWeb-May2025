namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StatusManagement;
    using HotelApp.Web.ViewModels.Admin.BookingManagement;


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
                .AsNoTracking()
                .Select(s => new StatusManagementIndexViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
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
    }
}

