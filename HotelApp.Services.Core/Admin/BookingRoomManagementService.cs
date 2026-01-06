namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using HotelApp.Data.Models;
    using HotelApp.Data.Repository.Interfaces;
    using HotelApp.Services.Core.Admin.Interfaces;
    using System;
    using System.Threading.Tasks;
    using HotelApp.GCommon;
    using HotelApp.Services.Common.Extensions;
    using HotelApp.Web.ViewModels.Admin.BookingRoomManagement;

    public class BookingRoomManagementService : IBookingRoomManagementService
    {
        private readonly IBookingRoomRepository bookingRoomRepository;
        private readonly IStatusManagementService statusService;

        public BookingRoomManagementService(IBookingRoomRepository bookingRoomRepository, 
            IStatusManagementService statusService)
        {
            this.bookingRoomRepository = bookingRoomRepository;
            this.statusService = statusService;
        }

        public async Task<BookingRoom?> FindBookingRoomByIdAsync(Guid id)
        {
            return await this.bookingRoomRepository
                .GetAllAttached()
                .Include(br => br.Status)
                .Include(br => br.Booking)
                    .ThenInclude(b => b.Status)
                .Include(br => br.Room)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<IEnumerable<BookingRoomManagementIndexViewModel>> GetBookingRoomManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize)
        {
            var query = this.bookingRoomRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Include(br => br.Status)
                .Include(br => br.Booking)
                .AsNoTracking()
                .OrderByDescending(br => br.Booking.CreatedOn)
                .Select(br => new BookingRoomManagementIndexViewModel
                {
                    Id = br.Id.ToString(),
                    CreatedOn = br.Booking.CreatedOn.ToHotelTime(),
                    BookingId = br.BookingId,
                    RoomId = br.RoomId,
                    Status = br.Status.Name,
                    IsDeleted = br.IsDeleted
                });

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalBookingRoomsCountAsync()
        {
            return await bookingRoomRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .CountAsync();
        }

        public async Task<BookingRoomManagementDetailsViewModel?> GetBookingRoomManagementDetailsByIdAsync(string? id)
        {
            BookingRoomManagementDetailsViewModel? bookingRoomDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid bookingRoomId);

            if (isIdValidGuid)
            {
                bookingRoomDetails = await this.bookingRoomRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Include(br => br.Status)
                    .Include(br => br.Booking)
                    .Include(br => br.Room)
                    .Where(br => br.Id == bookingRoomId)
                    .OrderByDescending(br => br.Booking.CreatedOn)
                    .Select(br => new BookingRoomManagementDetailsViewModel
                    {
                        Id = br.Id.ToString(),
                        CreatedOn = br.Booking.CreatedOn.ToHotelTime(),
                        BookingId = br.BookingId,
                        RoomId = br.RoomId,
                        AdultsCountPerRoom = br.AdultsCount,
                        ChildCountPerRoom = br.ChildCount,
                        BabyCountPerRoom = br.BabyCount,
                        Status = br.Status.Name,
                        IsDeleted = br.IsDeleted
                    })
                    .SingleOrDefaultAsync();
            }

            return bookingRoomDetails;
        }

        public async Task<BookingRoomManagementEditFormModel?> GetBookingRoomEditFormModelAsync(string? id)
        {
            BookingRoomManagementEditFormModel? formModel = null;
            if (!String.IsNullOrWhiteSpace(id))
            {
                BookingRoom? bookingRoomToEdit = await this.bookingRoomRepository
                    .GetAllAttached()
                    .Include(br => br.Room)
                        .ThenInclude(r => r.Category)
                    .Include(br => br.Booking)
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(br => br.Id.ToString().ToLower() == id.ToLower());

                if (bookingRoomToEdit != null)
                {
                    formModel = new BookingRoomManagementEditFormModel()
                    {
                        Id = bookingRoomToEdit.Id.ToString(),
                        AdultsCount = bookingRoomToEdit.AdultsCount,
                        ChildCount = bookingRoomToEdit.ChildCount,
                        BabyCount = bookingRoomToEdit.BabyCount,
                        DateDeparture = bookingRoomToEdit.Booking.DateDeparture,
                        StatusId = bookingRoomToEdit.StatusId,
                        MaxGuests = bookingRoomToEdit.Room.Category.Beds
                    };
                }
            }

            return formModel;
        }

        public async Task<bool> EditBookingRoomAsync(BookingRoomManagementEditFormModel? inputModel)
        {
            if (inputModel == null)
            {
                return false;
            }

            BookingRoom? bookingRoomToEdit = await this.bookingRoomRepository
                    .GetAllAttached()
                    .Include(br => br.Room)
                        .ThenInclude(r => r.Category)
                    .Include(br => br.Booking)
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(br => br.Id == Guid.Parse(inputModel.Id));

            if (bookingRoomToEdit == null)
            {
                return false;
            }

            var allowedStatuses = await this.statusService
                .GetAllowedStatusesInBookingRoomEditAsync(bookingRoomToEdit.StatusId, bookingRoomToEdit.Booking.DateDeparture, bookingRoomToEdit.Id.ToString());

            if (!allowedStatuses.Any(s => s.Id == inputModel.StatusId))
            {
                return false;
            }

            bookingRoomToEdit.AdultsCount = inputModel.AdultsCount;
            bookingRoomToEdit.ChildCount = inputModel.ChildCount;
            bookingRoomToEdit.BabyCount = inputModel.BabyCount;
            bookingRoomToEdit.StatusId = inputModel.StatusId;

            return await this.bookingRoomRepository.UpdateAsync(bookingRoomToEdit);
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreBookingRoomAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                BookingRoom? bookingRoom = await this.bookingRoomRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(br => br.Id.ToString().ToLower() == id.ToLower());
                if (bookingRoom != null)
                {
                    if (bookingRoom.IsDeleted)
                    {
                        isRestored = true;
                    }

                    bookingRoom.IsDeleted = !bookingRoom.IsDeleted;

                    result = await this.bookingRoomRepository
                        .UpdateAsync(bookingRoom);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

    }
}
