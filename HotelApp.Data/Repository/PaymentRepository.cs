namespace HotelApp.Data.Repository
{
    using HotelApp.Data.Models;
    using HotelApp.Data.Repository.Interfaces;

    public class PaymentRepository : BaseRepository<Payment, Guid>, IPaymentRepository
    {
        public PaymentRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
