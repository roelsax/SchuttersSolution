using Schutters.Models;

namespace Schutters.Repositories
{
    public interface ILedenRepository
    {
        public IEnumerable<Lid> GetAll();
        public Lid? Get(long lidnummer);
        public void Add(Lid lid);
        public void Update(Lid lid);
        public void Delete(long lidnummer);
    }
}
