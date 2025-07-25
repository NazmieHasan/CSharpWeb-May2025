namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Room in the system")]
    public class Room
    {
        [Comment("Room identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Comment("Room name(number)")]
        public string Name { get; set; } = null!;

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if movie is deleted")]
        public bool IsDeleted { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
    }
}
