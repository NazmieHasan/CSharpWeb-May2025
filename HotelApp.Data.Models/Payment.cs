namespace HotelApp.Data.Models
{
   public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }

        public string Amount { get; set; } = null!;

        public string BookingId { get; set; } = null!;

        public string PaymentUserFullName { get; set; } = null!;

        public string PaymentUserPhoneNumber { get; set; } = null!;
    }
}
