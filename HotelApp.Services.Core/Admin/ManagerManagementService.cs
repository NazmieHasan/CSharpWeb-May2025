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
                .IgnoreQueryFilters()
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

        public async Task<ManagerManagementDetailsViewModel?> GetManagerManagementDetailsByIdAsync(string? id)
        {
            var manager = await managerRepository.GetAllAttached()
                .IgnoreQueryFilters()
                .Where(m => m.Id.ToString() == id)
                .Include(m => m.User)
                .Include(m => m.ManagedBookings)
                    .ThenInclude(b => b.Status)
                .Include(m => m.ManagedBookings)
                    .ThenInclude(b => b.Room)
                .FirstOrDefaultAsync();

            if (manager == null)
            {
                return null;
            }

            return new ManagerManagementDetailsViewModel
            {
                Id = manager.Id,
                IsDeleted = manager.IsDeleted,
                UserId = manager.UserId,
                Email = manager.User.Email,
                ManagedBookings = manager.ManagedBookings
                    .OrderByDescending(b => b.CreatedOn)
                    .ToList()
            };
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreManagerAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Manager? manager = await this.managerRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(m => m.Id.ToString().ToLower() == id.ToLower());
                if (manager != null)
                {
                    if (manager.IsDeleted)
                    {
                        isRestored = true;
                    }

                    manager.IsDeleted = !manager.IsDeleted;

                    result = await this.managerRepository
                        .UpdateAsync(manager);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

    }
}
