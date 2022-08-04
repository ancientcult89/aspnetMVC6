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

        public void CreateProduct(Product p)
        {
            _dbContext.Add(p);
            _dbContext.SaveChanges();
        }

        public void DeleteProduct(Product p)
        {
            _dbContext.Remove(p);
            _dbContext.SaveChanges();
        }

        public void SaveProduct(Product p)
        {
            _dbContext.SaveChanges();
        }
    }
}
