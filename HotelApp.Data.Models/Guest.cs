namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    public class Guest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }

        public string FirstName { get; set; } = null!;

        public string FamilyName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
    }
}
