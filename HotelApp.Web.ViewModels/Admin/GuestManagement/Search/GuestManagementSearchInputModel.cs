namespace HotelApp.Web.ViewModels.Admin.GuestManagement.Search
{
    public class GuestManagementSearchInputModel
    {
        public string? Id { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? FirstName { get; set; }

        public string? FamilyName { get; set; }

        public DateOnly? BirthDate { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
