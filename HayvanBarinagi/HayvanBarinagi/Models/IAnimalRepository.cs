namespace HayvanBarinagi.Models
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        void Update(Animal animal);
        void Save();
    }
}
