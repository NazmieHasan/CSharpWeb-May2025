﻿namespace HotelApp.Web.ViewModels.Booking
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteBookingViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        public DateTime? CreatedOn { get; set; }

        public DateOnly? DateArrival { get; set; }

        public DateOnly? DateDeparture { get; set; }

        public int? AdultsCount { get; set; }

        public int? ChildCount { get; set; }

        public int? BabyCount { get; set; }

        public string? RoomName { get; set; }
    }
}
