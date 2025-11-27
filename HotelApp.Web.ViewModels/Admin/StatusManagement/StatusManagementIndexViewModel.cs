namespace HotelApp.Web.ViewModels.Admin.StatusManagement
{
    public class StatusManagementIndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
