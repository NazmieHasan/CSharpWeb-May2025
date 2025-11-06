namespace HotelApp.Services.Core.Admin
{ 
    using HotelApp.Data.Repository.Interfaces;
    using HotelApp.Services.Core.Admin.Interfaces;
    using HotelApp.Web.ViewModels.Admin.GuestManagement;
    using HotelApp.Web.ViewModels.Booking;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GuestService : IGuestService
    {
        private readonly IGuestRepository guestRepository;

        public GuestService(IGuestRepository guestRepository)
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
                    FirstName = g.FirstName,
                    FamilyName = g.FamilyName,
                    PhoneNumber = g.PhoneNumber,
                })
                .ToListAsync()
                ?? Enumerable.Empty<GuestManagementIndexViewModel>();
        }
    } 
}
