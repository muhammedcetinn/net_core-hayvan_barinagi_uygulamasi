namespace HayvanBarinagi.Models
{
    public interface ICustomersRepository : IRepository<Customers>
    {
        void Update(Customers customers);
        void Save();
    }
}
