namespace HotelApp.Web.ViewModels.Admin.GuestManagement
{
    public class GuestManagementIndexViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string FamilyName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
