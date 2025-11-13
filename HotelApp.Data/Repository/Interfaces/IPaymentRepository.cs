namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IPaymentRepository
        : IRepository<Payment, Guid>, IAsyncRepository<Payment, Guid>
    {
    }
}
