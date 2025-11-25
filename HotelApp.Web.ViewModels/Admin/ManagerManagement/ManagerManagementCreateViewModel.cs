namespace HotelApp.Web.ViewModels.Admin.ManagerManagement
{
    using System.ComponentModel.DataAnnotations; 

    public class ManagerManagementCreateViewModel
    {
        [Required(ErrorMessage = ValidationMessages.Manager.ManagerEmailRequiredMessage)]
        [EmailAddress(ErrorMessage = ValidationMessages.Manager.ManagerEmailInvalidMessage)]
        public string? UserEmail { get; set; }
    }
}
