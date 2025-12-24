namespace HotelApp.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

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

        public DateOnly? BirthDate { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var birth = BirthDate!.Value;

                int age = today.Year - birth.Year;

                if (today < new DateOnly(today.Year, birth.Month, birth.Day))
                {
                    age--;
                }

                return age;
            }
        }

        public bool IsDeleted { get; set; }

        public ICollection<Stay> Stays { get; set; } = new HashSet<Stay>();
    }
}
