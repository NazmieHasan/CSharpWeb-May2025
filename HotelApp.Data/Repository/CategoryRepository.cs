namespace HotelApp.Data.Repository
{
    using Interfaces;
    using Models;

    public class CategoryRepository : BaseRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(HotelAppDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
