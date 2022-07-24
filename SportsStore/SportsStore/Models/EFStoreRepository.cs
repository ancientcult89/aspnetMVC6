namespace SportsStore.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private StoreDbContext _dbContext;
        public EFStoreRepository(StoreDbContext storeDbContext)
        {
            _dbContext = storeDbContext;
        }
        public IQueryable<Product> Products => _dbContext.Products;
    }
}
