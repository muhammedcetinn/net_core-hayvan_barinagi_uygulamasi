using HayvanBarinagi.Utility;

namespace HayvanBarinagi.Models
{
    public class GiveAnimalRepository : Repository<GiveAnimal>, IGiveAnimalRepository
    {
        private ApplicationDbContext _appDbContext;
        public GiveAnimalRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Update(GiveAnimal giveAnimal)
        {
            _appDbContext.Update(giveAnimal);
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
