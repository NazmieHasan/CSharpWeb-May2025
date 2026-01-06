namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.GuestManagement;
    using Web.ViewModels.Admin.GuestManagement.Search;
    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Web.ViewModels;
    using HotelApp.GCommon;

    using HotelApp.Services.Common.Extensions;

    public class GuestManagementService : IGuestManagementService
    {
        private readonly IGuestRepository guestRepository;

        public GuestManagementService(IGuestRepository guestRepository)
        {
            this.guestRepository = guestRepository;
        }

        public async Task AddGuestManagementAsync(GuestManagementCreateViewModel inputModel)
        {
            var guest = await this.guestRepository
                .GetAllAttached()
                .FirstOrDefaultAsync(g => g.Email == inputModel.Email);

            if (guest != null)
            {
                throw new InvalidOperationException(ValidationMessages.Guest.GuestExistMessage);
            }

            Guest newGuest = new Guest()
            {
                FirstName = inputModel.FirstName,
                FamilyName = inputModel.FamilyName,
                PhoneNumber = inputModel.PhoneNumber,
                Email = inputModel.Email,
                BirthDate = inputModel.BirthDate
            };

            await this.guestRepository.AddAsync(newGuest);
        }

        public async Task<IEnumerable<GuestManagementIndexViewModel>> GetGuestManagementBoardDataAsync(int pageNumber = 1, int pageSize = ApplicationConstants.AdminPaginationPageSize)
        {
            var query = this.guestRepository
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
                    IsDeleted = g.IsDeleted
                });


            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalGuestsCountAsync()
        {
            return await guestRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .CountAsync();
        }

        public async Task<GuestManagementDetailsViewModel?> GetGuestManagementDetailsByIdAsync(string? id)
        {
            GuestManagementDetailsViewModel? guestDetails = null;

            bool isIdValidGuid = Guid.TryParse(id, out Guid guestId);

            if (isIdValidGuid)
            {
                guestDetails = await this.guestRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Include(g => g.Stays)
                        .ThenInclude(s => s.Guest)
                    .Include(g => g.Stays)
                        .ThenInclude(s => s.BookingRoom)
                            .ThenInclude(br => br.Booking)
                    .Where(g => g.Id == guestId)
                    .Select(g => new GuestManagementDetailsViewModel()
                    {
                        Id = g.Id,
                        CreatedOn = g.CreatedOn.ToHotelTime(),
                        FirstName = g.FirstName,
                        FamilyName = g.FamilyName,
                        PhoneNumber = g.PhoneNumber,
                        Email = g.Email,
                        BirthDate = g.BirthDate!.Value,
                        IsDeleted = g.IsDeleted,
                        Stays = g.Stays
                        .OrderByDescending(s => s.CreatedOn)
                        .Select(s => new StayManagementDetailsViewModelInGuestDetails
                        {
                            Id = s.Id,
                            CreatedOn = s.CreatedOn.ToHotelTime(),
                            CheckoutOn = s.CheckoutOn.HasValue
                            ? s.CheckoutOn.Value.ToHotelTime()
                            : null,
                            IsDeleted = s.IsDeleted,
                            BookingId = s.BookingRoom.Booking.Id,
                        })
                        .ToList()
                    })
                    .SingleOrDefaultAsync();
            }

            return guestDetails;
        }

        public async Task<GuestManagementEditViewModel?> GetGuestEditFormModelAsync(string? id)
        {
            GuestManagementEditViewModel? formModel = null;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Guest? guestToEdit = await this.guestRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(g => g.Id.ToString().ToLower() == id.ToLower());
                if (guestToEdit != null)
                {
                    formModel = new GuestManagementEditViewModel()
                    {
                        Id = guestToEdit.Id.ToString(),
                        FirstName = guestToEdit.FirstName,
                        FamilyName = guestToEdit.FamilyName,
                        PhoneNumber = guestToEdit.PhoneNumber,
                        Email = guestToEdit.Email,
                        BirthDate = guestToEdit.BirthDate.Value
                    };
                }
            }

            return formModel;
        }

        public async Task<bool> EditGuestAsync(GuestManagementEditViewModel? inputModel)
        {
            bool result = false;

            if (inputModel != null)
            {
                Guest? guestToEdit = await this.guestRepository
                        .SingleOrDefaultAsync(g => g.Id.ToString().ToLower() == inputModel.Id.ToLower());
                
                if (guestToEdit != null)
                {
                    var existingGuest = await this.guestRepository
                        .GetAllAttached()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(g =>
                            g.Email.ToLower() == inputModel.Email.ToLower() &&
                            g.Id.ToString() != inputModel.Id);

                    if (existingGuest != null)
                    {
                        throw new InvalidOperationException(ValidationMessages.Guest.GuestExistMessage);
                    }

                    guestToEdit.FirstName = inputModel.FirstName;
                    guestToEdit.FamilyName = inputModel.FamilyName;
                    guestToEdit.PhoneNumber = inputModel.PhoneNumber;
                    guestToEdit.Email = inputModel.Email;
                    guestToEdit.BirthDate = inputModel.BirthDate;

                    result = await this.guestRepository
                        .UpdateAsync(guestToEdit);
                }
            }

            return result;
        }

        public async Task<Tuple<bool, bool>> DeleteOrRestoreGuestAsync(string? id)
        {
            bool result = false;
            bool isRestored = false;
            if (!String.IsNullOrWhiteSpace(id))
            {
                Guest? guest = await this.guestRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(g => g.Id.ToString() == id);
                if (guest != null)
                {
                    if (guest.IsDeleted)
                    {
                        isRestored = true;
                    }

                    guest.IsDeleted = !guest.IsDeleted;

                    result = await this.guestRepository
                        .UpdateAsync(guest);
                }
            }

            return new Tuple<bool, bool>(result, isRestored);
        }

        public async Task<IEnumerable<GuestManagementSearchResultViewModel>> SearchGuestAsync(GuestManagementSearchInputModel inputModel)
        {
            var query = guestRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AsQueryable();

            // Guest Id
            if (!string.IsNullOrWhiteSpace(inputModel.Id))
            {
                if (!Guid.TryParse(inputModel.Id, out Guid guestId))
                {
                    return new List<GuestManagementSearchResultViewModel>();
                }
                query = query.Where(g => g.Id == guestId);
            }

            // Created On
            if (inputModel.CreatedOn.HasValue)
            {
                var createdDate = inputModel.CreatedOn.Value.Date;

                query = query.Where(g =>
                    g.CreatedOn.Date == createdDate);
            }

            // First Name
            if (!string.IsNullOrWhiteSpace(inputModel.FirstName))
            {
                query = query.Where(g =>
                    g.FirstName.Contains(inputModel.FirstName));
            }

            // Family Name
            if (!string.IsNullOrWhiteSpace(inputModel.FamilyName))
            {
                query = query.Where(g =>
                    g.FamilyName.Contains(inputModel.FamilyName));
            }

            // Birth Date
            if (inputModel.BirthDate.HasValue)
            {
                query = query.Where(g =>
                    g.BirthDate == inputModel.BirthDate.Value);
            }

            // IsDeleted
            if (inputModel.IsDeleted.HasValue)
            {
                query = query.Where(g =>
                    g.IsDeleted == inputModel.IsDeleted.Value);
            }

            var guests = await query
                .OrderByDescending(g => g.CreatedOn)
                .Select(g => new GuestManagementSearchResultViewModel
                {
                    Id = g.Id.ToString(),
                    CreatedOn = g.CreatedOn,
                    FirstName = g.FirstName,
                    FamilyName = g.FamilyName,
                    PhoneNumber = g.PhoneNumber,
                    IsDeleted = g.IsDeleted
                })
                .ToListAsync();

            return guests;
        }

    } 
}
