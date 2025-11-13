namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StayManagement;

    public class StayManagementService : IStayManagementService
    {
        private readonly IStayRepository stayRepository;

        public StayManagementService(IStayRepository stayRepository)
        {
            this.stayRepository = stayRepository;
        }

        public async Task<IEnumerable<StayManagementIndexViewModel>> GetStayManagementBoardDataAsync()
        {
            return await stayRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(s => new StayManagementIndexViewModel
                {
                    Id = s.Id,
                    CreatedOn = s.CreatedOn,
                })
                .ToListAsync()
                ?? Enumerable.Empty<StayManagementIndexViewModel>();
        }
    }
}
