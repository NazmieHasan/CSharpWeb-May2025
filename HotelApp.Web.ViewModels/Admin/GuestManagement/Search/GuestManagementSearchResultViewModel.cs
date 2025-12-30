namespace HotelApp.Web.ViewModels.Admin.GuestManagement.Search
{
    public class GuestManagementSearchResultViewModel
    {
        public string Id { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string FirstName { get; set; } = null!;
        public string FamilyName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}