using HayvanBarinagi.Utility;

namespace HayvanBarinagi.Models
{
    public class CustomersRepository : Repository<Customers>, ICustomersRepository
    {
        private ApplicationDbContext _appDbContext;
        public CustomersRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Update(Customers customers)
        {
            _appDbContext.Update(customers);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
