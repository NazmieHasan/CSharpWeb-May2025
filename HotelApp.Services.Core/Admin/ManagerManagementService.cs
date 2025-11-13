namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.ManagerManagement;

    public class ManagerManagementService : IManagerManagementService
    {
        private readonly IManagerRepository managerRepository;

        public ManagerManagementService(IManagerRepository managerRepository)
        {
            this.managerRepository = managerRepository;
        }

        public async Task<IEnumerable<ManagerManagementIndexViewModel>> GetManagerManagementBoardDataAsync()
        {
            return await managerRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(m => new ManagerManagementIndexViewModel
                {
                    Id = m.Id,
                    IsDeleted = m.IsDeleted,
                    Email = m.User.Email
                })
                .ToListAsync()
                ?? Enumerable.Empty<ManagerManagementIndexViewModel>();
        }
    }
}
