namespace HotelApp.Web.ViewModels.Room
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteRoomViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        public string? Name { get; set; }

        public string? CategoryName { get; set; }
    }
}
