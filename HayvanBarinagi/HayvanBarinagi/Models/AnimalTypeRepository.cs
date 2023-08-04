using HayvanBarinagi.Utility;

namespace HayvanBarinagi.Models
{
    public class AnimalTypeRepository : Repository<AnimalType>, IAnimalTypeRepository
    {
        private ApplicationDbContext _appDbContext;
        public AnimalTypeRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Update(AnimalType animalType)
        {
            _appDbContext.Update(animalType);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
