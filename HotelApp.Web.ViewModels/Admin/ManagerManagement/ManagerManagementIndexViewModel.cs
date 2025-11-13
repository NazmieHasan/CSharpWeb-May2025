namespace HotelApp.Web.ViewModels.Admin.ManagerManagement
{
    public class ManagerManagementIndexViewModel
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }

        public string Email { get; set; } = null!;
    }
}
