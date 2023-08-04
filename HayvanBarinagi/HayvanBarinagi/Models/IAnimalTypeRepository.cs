namespace HayvanBarinagi.Models
{
    public interface IAnimalTypeRepository : IRepository<AnimalType>
    {
        void Update(AnimalType animalType);
        void Save();
    }
}
