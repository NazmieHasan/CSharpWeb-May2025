namespace HotelApp.Services.Core.Admin
{
    using Microsoft.EntityFrameworkCore;

    using Data.Models;
    using Data.Repository.Interfaces;
    using Interfaces;
    using Web.ViewModels.Admin.GuestManagement;
    using HotelApp.Web.ViewModels.Admin.StayManagement;
    using HotelApp.Web.ViewModels;

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
                    IsDeleted = g.IsDeleted
                })
                .ToListAsync()
                ?? Enumerable.Empty<GuestManagementIndexViewModel>();
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
                        .ThenInclude(s => s.Booking)
                    .Where(g => g.Id == guestId)
                    .Select(g => new GuestManagementDetailsViewModel()
                    {
                        Id = g.Id,
                        CreatedOn = g.CreatedOn,
                        FirstName = g.FirstName,
                        FamilyName = g.FamilyName,
                        PhoneNumber = g.PhoneNumber,
                        Email = g.Email,
                        BirthDate = g.BirthDate.Value,
                        IsDeleted = g.IsDeleted,
                        Stays = g.Stays.Select(s => new StayManagementDetailsViewModelInGuestDetails
                        {
                            Id = s.Id,
                            CreatedOn = s.CreatedOn,
                            CheckoutOn = s.CheckoutOn,
                            IsDeleted = s.IsDeleted,
                            BookingId = s.BookingId,
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

    } 
}
