namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.ManagerManagement;
    using HotelApp.Data.Models;
    using HotelApp.Web.ViewModels;

    public class ManagerManagementService : IManagerManagementService
    {
        private readonly IManagerRepository managerRepository;
        private readonly IUserRepository userRepository;

        public ManagerManagementService(IManagerRepository managerRepository,
            IUserRepository userRepository)
        {
            this.managerRepository = managerRepository;
            this.userRepository = userRepository;
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

        public async Task AddManagerManagementAsync(ManagerManagementCreateViewModel inputModel)
        {
            var user = await this.userRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(u => u.Email == inputModel.UserEmail);

            if (user == null)
            {
                throw new ArgumentException(ValidationMessages.Manager.ManagerEmailNotFoundMessage);
            }

            var existingManager = await this.managerRepository
                .GetAllAttached()
                .AnyAsync(m => m.UserId == user.Id);

            if (existingManager)
            {
                throw new ArgumentException(ValidationMessages.Manager.ManagerExistMessage);
            }

            var newManager = new Manager
            {
                UserId = user.Id 
            };

            await this.managerRepository.AddAsync(newManager);
        }

    }
}
