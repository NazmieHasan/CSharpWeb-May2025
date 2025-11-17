namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    public class Guest
    {
        public Guest()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }
        public Guid Id { get; set; } 

        public DateTime CreatedOn { get; set; }

        public string FirstName { get; set; } = null!;

        public string FamilyName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public ICollection<Stay> Stays { get; set; } = new HashSet<Stay>();
    }
}
