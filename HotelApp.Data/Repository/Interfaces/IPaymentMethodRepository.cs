namespace HotelApp.Data.Repository.Interfaces
{
    using Models;

    public interface IPaymentMethodRepository
        : IRepository<PaymentMethod, int>, IAsyncRepository<PaymentMethod, int>
    {
    }
}
