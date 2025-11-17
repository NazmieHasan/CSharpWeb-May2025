namespace HotelApp.Data.Models
{
    using Microsoft.EntityFrameworkCore;

    [Comment("Payment method in the system")]
    public class PaymentMethod
    {
        [Comment("Payment identifier")]
        public int Id { get; set; }

        [Comment("Payment name")]
        public string Name { get; set; } = null!;

        // TODO: Extract the property with Id to BaseDeletableModel
        [Comment("Shows if payment method is deleted")]
        public bool IsDeleted { get; set; }

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
