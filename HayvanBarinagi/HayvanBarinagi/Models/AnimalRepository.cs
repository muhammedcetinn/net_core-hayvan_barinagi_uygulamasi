using HayvanBarinagi.Utility;

namespace HayvanBarinagi.Models
{
    public class AnimalRepository : Repository<Animal>, IAnimalRepository
    {
        private ApplicationDbContext _appDbContext;
        public AnimalRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Update(Animal animal)
        {
            _appDbContext.Update(animal);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
