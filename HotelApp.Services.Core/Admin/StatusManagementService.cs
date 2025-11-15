namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StatusManagement;

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
    }
}

