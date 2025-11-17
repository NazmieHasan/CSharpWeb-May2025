namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.GuestManagement;
		
    public class GuestManagementService : IGuestManagementService
    {
        private readonly IGuestRepository guestRepository;

        public GuestManagementService(IGuestRepository guestRepository)
        {
            this.guestRepository = guestRepository;
        }

        public async Task AddGuestManagementAsync(GuestManagementCreateViewModel inputModel)
        {
            Guest newGuest = new Guest()
            {
                FirstName = inputModel.FirstName,
                FamilyName = inputModel.FamilyName,
                PhoneNumber = inputModel.PhoneNumber,
                Email = inputModel.Email,
            };

            await this.guestRepository.AddAsync(newGuest);
        }

        public async Task<IEnumerable<GuestManagementIndexViewModel>> GetGuestManagementBoardDataAsync()
        {
            return await guestRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .OrderByDescending(g => g.CreatedOn)
                .Select(g => new GuestManagementIndexViewModel
                {
                    Id = g.Id,
                    FirstName = g.FirstName,
                    FamilyName = g.FamilyName,
                    PhoneNumber = g.PhoneNumber,
                })
                .ToListAsync()
                ?? Enumerable.Empty<GuestManagementIndexViewModel>();
        }
    } 
}
