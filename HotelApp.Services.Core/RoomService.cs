namespace HotelApp.Services.Core
{

    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Interfaces;
    using HotelApp.Web.ViewModels.Room;

    public class RoomService : IRoomService
    {
        private readonly HotelAppDbContext dbContext;

        public RoomService(HotelAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddRoomAsync(AddRoomInputModel inputModel)
        {
            bool opRes = false;

            Category? catRef = await this.dbContext
                .Categories
                .FindAsync(inputModel.CategoryId);

            if (catRef != null)
            {
                Room newRecipe = new Room()
                {
                    Name = inputModel.Name,
                    CategoryId = inputModel.CategoryId
                };

                await this.dbContext.Rooms.AddAsync(newRecipe);
                await this.dbContext.SaveChangesAsync();

                opRes = true;
            }

            return opRes;
        }

        public async Task<bool> DeleteRoomAsync(string? id)
        {
            Room? roomToDelete = await this.FindRoomByStringId(id);
            if (roomToDelete == null)
            {
                return false;
            }

            // TODO: To be investigated when relations to Room entity are introduced
            this.dbContext.Rooms.Remove(roomToDelete);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AllRoomsIndexViewModel>> GetAllRoomsAsync()
        {
            IEnumerable<AllRoomsIndexViewModel> allRooms = await this.dbContext
                .Rooms
                .Include(r => r.Category)
                .Where(r => !r.IsDeleted)
                .Select(r => new AllRoomsIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    Category = r.Category.Name,
                    CategoryId = r.CategoryId,
                })
                .ToArrayAsync();

            return allRooms;
        }

        public async Task<DeleteRoomViewModel?> GetRoomDeleteDetailsByIdAsync(string? id)
        {
            DeleteRoomViewModel? deleteRoomViewModel = null;

            Room? roomToBeDeleted = await this.FindRoomByStringId(id);
            if (roomToBeDeleted != null)
            {
                deleteRoomViewModel = new DeleteRoomViewModel()
                {
                    Id = roomToBeDeleted.Id.ToString(),
                    Name = roomToBeDeleted.Name,
                    CategoryName = roomToBeDeleted.Category.Name
                };
            }

            return deleteRoomViewModel;
        }

        public async Task<RoomDetailsViewModel?> GetRoomDetailsByIdAsync(string? id)
        {
            RoomDetailsViewModel? roomDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid roomId);

            if (isIdValidGuid)
            {
                roomDetails = await this.dbContext
                    .Rooms
                    .Include(r => r.Category)
                    .AsNoTracking()
                    .Where(r => r.Id == roomId)
                    .Select(r => new RoomDetailsViewModel()
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        Category = r.Category.Name
                    })
                    .SingleOrDefaultAsync();
            }

            return roomDetails;
        }

        public async Task<EditRoomInputModel?> GetRoomForEditAsync(string? id)
        {
            EditRoomInputModel? editModel = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid roomId);

            if (isIdValidGuid)
            {
                editModel = await this.dbContext
                    .Rooms
                    .AsNoTracking()
                    .Where(r => r.Id == roomId)
                    .Select(r => new EditRoomInputModel()
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        CategoryId = r.CategoryId
                    })
                    .SingleOrDefaultAsync();
            }

            return editModel;
        }

        public async Task<bool> PersistUpdatedRoomAsync(EditRoomInputModel inputModel)
        {
            Room? editableRoom = await this.FindRoomByStringId(inputModel.Id);

            if (editableRoom == null)
            {
                return false;
            }

            editableRoom.Name = inputModel.Name;
            editableRoom.CategoryId = inputModel.CategoryId;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SoftDeleteRoomAsync(string? id)
        {
            Room? roomToDelete = await this.FindRoomByStringId(id);
            if (roomToDelete == null)
            {
                return false;
            }

            // Soft Delete <=> Edit of IsDeleted property
            roomToDelete.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();

            return true;
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
                    room = await this.dbContext
                        .Rooms
                        .Include(r => r.Category)
                        .FirstOrDefaultAsync(r => r.Id == roomGuid);
                }
            }

            return room;
        }
    }
}
