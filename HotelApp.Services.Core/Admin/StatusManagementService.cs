namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;

    using HotelApp.Web.ViewModels.Admin.StatusManagement;
    using HotelApp.Web.ViewModels.Admin.BookingManagement;
    using HotelApp.Web.ViewModels.Admin.BookingRoomManagement;
    using HotelApp.Web.ViewModels;


    public class StatusManagementService : IStatusManagementService
    {
        private readonly IStatusRepository statusRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IBookingRoomRepository bookingRoomRepository;

        public StatusManagementService(IStatusRepository statusRepository,
            IBookingRepository bookingRepository,
            IBookingRoomRepository bookingRoomRepository)
        {
            this.statusRepository = statusRepository;
            this.bookingRepository = bookingRepository;
            this.bookingRoomRepository = bookingRoomRepository;
        }

        public async Task<IEnumerable<StatusManagementIndexViewModel>> GetStatusManagementBoardDataAsync()
        {
            return await statusRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(s => new StatusManagementIndexViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsDeleted = s.IsDeleted
                })
                .ToListAsync()
                ?? Enumerable.Empty<StatusManagementIndexViewModel>();
        }

        // not added GetStatusesDropDownDataAsync() to IStatusRepository because the method
        // use a ViewModel, includes a projection
        // TO DO: Move the projection to the repository as a helper method (but not part of the public interface),
        // which returns IQueryable<Status> or an anonymous DTO,
        // and then apply .Select(...) to a ViewModel in the service layer.
        public async Task<IEnumerable<AddBookingStatusDropDownModel>> GetBookingStatusesDropDownDataAsync()
        {
            IEnumerable<AddBookingStatusDropDownModel> statusesAsDropDown = await this.statusRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(s => new AddBookingStatusDropDownModel()
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToArrayAsync();

            return statusesAsDropDown;
        }

        public async Task<IEnumerable<AddBookingRoomStatusDropDownModel>> GetBookingRoomStatusesDropDownDataAsync()
        {
            IEnumerable<AddBookingRoomStatusDropDownModel> statusesAsDropDown = await this.statusRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(s => new AddBookingRoomStatusDropDownModel()
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToArrayAsync();

            return statusesAsDropDown;
        }

        public async Task<IEnumerable<AddBookingStatusDropDownModel>> GetAllowedStatusesInBookingEditAsync(int currentStatusId, DateOnly dateDeparture, string bookingIdString)
        {
            var allStatuses = await this.GetBookingStatusesDropDownDataAsync();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (!Guid.TryParse(bookingIdString, out Guid bookingId))
            {
                return Enumerable.Empty<AddBookingStatusDropDownModel>();
            }

            var booking = await bookingRepository
                .GetAllAttached()
                .Include(b => b.Status)
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Stays)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            return currentStatusId switch
            {
                // Awaiting Payment = 1
                1 => allStatuses.Where(s => 
                    (s.Name == "Cancelled")
                    || s.Id == currentStatusId),

                // For Implementation = 3
                3 => allStatuses.Where(s =>
                    (s.Name == "Cancelled" && today < dateDeparture) ||
                    (s.Name == "Done" && dateDeparture == today)
                    || s.Id == currentStatusId),

                // In Progress = 4
                4 => allStatuses.Where(s =>
                    (s.Name == "Done" && dateDeparture == today &&
                    booking.BookingRooms.All(br => br.Stays.Count() < (booking.AdultsCount + booking.ChildCount + booking.BabyCount)))
                    || s.Id == currentStatusId),

                _ => allStatuses.Where(s => s.Id == currentStatusId)
            };
        }

        public async Task<IEnumerable<AddBookingRoomStatusDropDownModel>> GetAllowedStatusesInBookingRoomEditAsync(int currentStatusId, DateOnly dateDeparture, string bookingRoomIdString)
        {
            var allStatuses = await this.GetBookingRoomStatusesDropDownDataAsync();

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (!Guid.TryParse(bookingRoomIdString, out Guid bookingRoomId))
            {
                return Enumerable.Empty<AddBookingRoomStatusDropDownModel>();
            }

            var bookingRoom = await this.bookingRoomRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(br => br.Booking)   
                .Include(br => br.Stays)
                .FirstOrDefaultAsync(br => br.Id == bookingRoomId);

            if (bookingRoom == null)
            {
                return allStatuses.Where(s => s.Id == currentStatusId);
            }

            return currentStatusId switch
            {
                // Awaiting Payment = 1
                1 => allStatuses.Where(s =>
                    s.Name == "Cancelled"
                    || s.Id == currentStatusId),

                // For Implementation = 3
                3 => allStatuses.Where(s =>
                    (s.Name == "Cancelled" && today < dateDeparture)
                    || (s.Name == "Done - No Guests" && dateDeparture == today)
                    || s.Id == currentStatusId),

                // In Progress = 4
                4 => allStatuses.Where(s =>
                    (s.Name == "Done - Partial Attendance"
                        && dateDeparture == today
                        && bookingRoom.Booking.BookingRooms   
                            .All(br => br.Stays.Count() <
                                       (br.AdultsCount + br.ChildCount + br.BabyCount)))
                    || s.Id == currentStatusId),

                _ => allStatuses.Where(s => s.Id == currentStatusId)
            };
        }

        public async Task AddStatusManagementAsync(StatusManagementFormInputModel inputModel)
        {
            var existingStatus = await this.statusRepository
                .GetAllAttached()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name.ToLower() == inputModel.Name.ToLower());

            if (existingStatus != null)
            {
                throw new InvalidOperationException(ValidationMessages.Status.NameAlreadyExistsMessage);
            }

            Status newStatus = new Status()
            {
                Name = inputModel.Name
            };

            await this.statusRepository.AddAsync(newStatus);
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreStatusAsync(int? id)
        {
            bool result = false;
            bool isRestored = false;
            if (id > 0)
            {
                Status? status = await this.statusRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(s => s.Id == id);
                if (status != null)
                {
                    if (status.IsDeleted)
                    {
                        isRestored = true;
                    }

                    status.IsDeleted = !status.IsDeleted;

                    result = await this.statusRepository
                        .UpdateAsync(status);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

    }
}

