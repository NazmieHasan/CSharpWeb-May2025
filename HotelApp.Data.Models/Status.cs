namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Booking status in the system")]

    public class Status
    {
        [Comment("Status identifier")]
        public int Id { get; set; }

        [Comment("Status name")]
        public string Name { get; set; } = null!;

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if status is deleted")]
        public bool IsDeleted { get; set; }
    }
}
