namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.RoomManagement;

    using static HotelApp.Web.ViewModels.ValidationMessages.Room;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels;
    using HotelApp.Web.ViewModels.Room;

    public class RoomManagementService : IRoomManagementService
    {
        private readonly IRoomRepository roomRepository;
        private readonly ICategoryRepository categoryRepository;

        public RoomManagementService(IRoomRepository roomRepository, 
            ICategoryRepository categoryRepository)
        {
            this.roomRepository = roomRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<bool> AddRoomManagementAsync(AddRoomManagementInputModel inputModel)
        {
            bool opRes = false;

            Category? catRef = await this.categoryRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(c => c.Id == inputModel.CategoryId);

            if (catRef != null)
            {
                Room newRoom = new Room()
                {
                    Name = inputModel.Name,
                    CategoryId = inputModel.CategoryId
                };

                await this.roomRepository.AddAsync(newRoom);

                opRes = true;
            }

            return opRes;
        }

        public async Task<IEnumerable<RoomManagementIndexViewModel>> GetRoomManagementBoardDataAsync()
        {
            return await roomRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Select(r => new RoomManagementIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    IsDeleted = r.IsDeleted
                })
                .ToListAsync()
                ?? Enumerable.Empty<RoomManagementIndexViewModel>();
        }

        public async Task<RoomManagementDetailsViewModel?> GetRoomDetailsByIdAsync(string? id)
        {
            RoomManagementDetailsViewModel? roomDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid roomId);

            if (isIdValidGuid)
            {
                roomDetails = await this.roomRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .Include(r => r.Category)
                    .AsNoTracking()
                    .Where(r => r.Id == roomId)
                    .Select(r => new RoomManagementDetailsViewModel()
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        Category = r.Category.Name,
                        CategoryBeds = r.Category.Beds,
                        IsDeleted = r.IsDeleted
                    })
                    .SingleOrDefaultAsync();
            }

            return roomDetails;
        }

        public async Task<EditRoomManagementInputModel?> GetRoomForEditAsync(string? id)
        {
            EditRoomManagementInputModel? editModel = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid roomId);

            if (isIdValidGuid)
            {
                editModel = await this.roomRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .Where(r => r.Id == roomId)
                    .Select(r => new EditRoomManagementInputModel()
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        CategoryId = r.CategoryId
                    })
                    .SingleOrDefaultAsync();
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedRoomAsync(EditRoomManagementInputModel inputModel)
        {
            Room? editableRoom = await this.FindRoomByStringId(inputModel.Id);

            if (editableRoom == null)
            {
                return false;
            }

            editableRoom.Name = inputModel.Name;
            editableRoom.CategoryId = inputModel.CategoryId;

            await this.roomRepository.UpdateAsync(editableRoom);

            return true;
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreRoomAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;

            if (!String.IsNullOrWhiteSpace(id))
            {
                Room? room = await this.roomRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(r => r.Id.ToString().ToLower() == id.ToLower());
                if (room != null)
                {
                    if (room.IsDeleted)
                    {
                        isRestored = true;
                    }

                    room.IsDeleted = !room.IsDeleted;

                    result = await this.roomRepository
                        .UpdateAsync(room);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

        // TODO: Implement as generic method in BaseService
        private async Task<Room?> FindRoomByStringId(string? id)
        {
            Room? room = null;

            if (!string.IsNullOrWhiteSpace(id))
            {
                bool isGuidValid = Guid.TryParse(id, out Guid roomGuid);
                if (isGuidValid)
                {
                    room = await this.roomRepository
                    .GetByIdAsync(roomGuid);
                }
            }

            return room;
        }

    }
}
