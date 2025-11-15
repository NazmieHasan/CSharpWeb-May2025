namespace HotelApp.Data.Repository
{
    using HotelApp.Data.Models;
    using HotelApp.Data.Repository.Interfaces;

    public class PaymentMethodRepository : BaseRepository<PaymentMethod, int>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
