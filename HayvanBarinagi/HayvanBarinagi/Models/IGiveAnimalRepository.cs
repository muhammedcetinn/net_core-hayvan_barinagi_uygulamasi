namespace HayvanBarinagi.Models
{
    public interface IGiveAnimalRepository : IRepository<GiveAnimal>
    {
        void Update(GiveAnimal giveAnimal);
        void Save();
    }
}
