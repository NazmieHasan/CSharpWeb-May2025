namespace HotelApp.Web.ViewModels.Admin.StayManagement
{
    public class StayManagementIndexViewModel
    {
        public Guid Id { get; set; }

        public string GuestNames { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
