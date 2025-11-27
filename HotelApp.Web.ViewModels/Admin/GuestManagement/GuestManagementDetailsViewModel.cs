namespace HotelApp.Web.ViewModels.Admin.GuestManagement
{
    using HotelApp.Data.Models;
    using HotelApp.Web.ViewModels.Admin.StayManagement;

    public class GuestManagementDetailsViewModel : GuestManagementIndexViewModel
    {
        public DateTime CreatedOn { get; set; }

        public string Email { get; set; } = null!;

        public ICollection<StayManagementDetailsViewModelInGuestDetails> Stays { get; set; }
            = new List<StayManagementDetailsViewModelInGuestDetails>();
    }
}
