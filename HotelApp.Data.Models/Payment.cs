namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedOn { get; set; }

        public decimal Amount { get; set; }

        public Guid BookingId { get; set; }

        public Booking Booking { get; set; } = null!;

        public string PaymentUserFullName { get; set; } = null!;

        public string PaymentUserPhoneNumber { get; set; } = null!;

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if payment is deleted")]
        public bool IsDeleted { get; set; }

        public int PaymentMethodId { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = null!;
    }
}
