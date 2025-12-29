namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;

    using Web.ViewModels.Admin.RoomManagement;
    using Web.ViewModels.Admin.BookingManagement;

    using static GCommon.ApplicationConstants;
    using HotelApp.Web.ViewModels;
    using HotelApp.GCommon;

    public class RoomManagementService : IRoomManagementService
    {
        private readonly IRoomRepository roomRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBookingRepository bookingRepository;

        public RoomManagementService(IRoomRepository roomRepository, 
            ICategoryRepository categoryRepository,
            IBookingRepository bookingRepository)
        {
            this.roomRepository = roomRepository;
            this.categoryRepository = categoryRepository;
            this.bookingRepository = bookingRepository;
        }

        public async Task<bool> AddRoomManagementAsync(AddRoomManagementInputModel inputModel)
        {
            bool opRes = false;

            var existingRoom = await this.roomRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name.ToLower() == inputModel.Name.ToLower());

            if (existingRoom != null)
            {
                throw new InvalidOperationException(ValidationMessages.Room.NameAlreadyExistsMessage);
            }

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

        public async Task<IEnumerable<RoomManagementIndexViewModel>> GetRoomManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize)
        {
            var query = this.roomRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .Select(r => new RoomManagementIndexViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    IsDeleted = r.IsDeleted
                });

            return await query
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
        }

        public async Task<int> GetTotalRoomsCountAsync()
        {
            return await roomRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .CountAsync();
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
                    .Include(r => r.Bookings)
                        .ThenInclude(b => b.Status) 
                    .AsNoTracking()
                    .Where(r => r.Id == roomId)
                    .Select(r => new RoomManagementDetailsViewModel()
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        CategoryId = r.CategoryId,
                        Category = r.Category.Name,
                        CategoryBeds = r.Category.Beds,
                        IsDeleted = r.IsDeleted,
                        Bookings = r.Bookings
                            .OrderByDescending(b => b.DateArrival)
                            .Select(b => new BookingInfoViewModel
                            {
                                BookingId = b.Id.ToString(),
                                DateArrival = b.DateArrival,
                                DateDeparture = b.DateDeparture,
                                CreatedOn = b.CreatedOn,
                                Status = b.Status.Name
                            })
                            .ToList()
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

            var existingRoom = await this.roomRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.Name.ToLower() == inputModel.Name.ToLower() &&
                    r.Id.ToString() != inputModel.Id);

            if (existingRoom != null)
            {
                throw new InvalidOperationException(ValidationMessages.Room.NameAlreadyExistsMessage);
            }

            if (editableRoom.CategoryId != inputModel.CategoryId)
            {
                var newCategory = await this.categoryRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == inputModel.CategoryId);

                if (newCategory == null)
                {
                    throw new InvalidOperationException(ValidationMessages.Room.CategoryRequiredMessage);
                }

                bool hasConflictingBookings = await this.bookingRepository
                    .GetAllAttached()
                    .Where(b => b.RoomId == editableRoom.Id)
                    .AnyAsync(b => (b.AdultsCount + b.ChildCount + b.BabyCount) > newCategory.Beds + 1);

                if (hasConflictingBookings)
                {
                    throw new InvalidOperationException(ValidationMessages.Room.CategoryCannotBeChangedMessage);
                }

                editableRoom.CategoryId = inputModel.CategoryId;
            }

            editableRoom.Name = inputModel.Name;

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
