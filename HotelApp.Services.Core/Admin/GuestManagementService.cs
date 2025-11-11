namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<GuestManagementIndexViewModel>> GetGuestManagementBoardDataAsync()
        {
            return await guestRepository
                .GetAllAttached()
                .AsNoTracking()
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
