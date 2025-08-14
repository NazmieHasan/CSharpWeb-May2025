namespace HotelApp.Data.Models
{
    public class Stay
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }

        public string GuestId { get; set; } = null!;

        public string BookingId { get; set; } = null!;
    }
}
