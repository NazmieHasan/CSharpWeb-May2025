﻿namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Booking in the system")]
    public class Booking
    {
        [Comment("Booking identifier")]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }
        
        public DateOnly DateArrival { get; set; }

        public DateOnly DateDeparture { get; set; }

        public int AdultsCount { get; set; }

        [Comment("Child age is between 4 and 17")]
        public int ChildCount { get; set; }

        [Comment("Baby age is between 0 and 3")]
        public int BabyCount { get; set; }

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if booking is deleted")]
        public bool IsDeleted { get; set; }

        public Guid RoomId { get; set; }

        public virtual Room Room { get; set; } = null!;
    }
}
