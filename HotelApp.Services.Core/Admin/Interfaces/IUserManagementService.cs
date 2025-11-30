namespace HotelApp.Services.Core.Admin.Interfaces
{
    using Web.ViewModels.Admin.UserManagement;

    public interface IUserManagementService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetUserManagementBoardDataAsync(string userId);

        Task<IEnumerable<string>> GetManagerEmailsAsync();

        Task<bool> AssignUserToRoleAsync(RoleSelectionInputModel inputModel);

        Task<bool> RemoveUserFromRoleAsync(string userId, string role);

        Task<UserManagementDetailsViewModel?> GetUserManagementDetailsByIdAsync(string? id);

    }
}
