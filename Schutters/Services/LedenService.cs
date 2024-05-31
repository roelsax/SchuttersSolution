using Schutters.Repositories;
using Schutters.Models;

namespace Schutters.Services
{
    public class LedenService
    {
        private readonly ILedenRepository repository;

        public LedenService(ILedenRepository repository)
        {
            this.repository = repository;
        }

        public Lid? FindLid(long lidnummer) => repository.Get(lidnummer);

        public IEnumerable<Lid> GetLeden() => repository.GetAll();

        public void Create(Lid lid) => repository.Add(lid);
        public void Update(Lid lid) => repository.Update(lid);
        public void Remove(long lidnummer) => repository.Delete(lidnummer);

        public bool Bestaat(long? lidnummer)
        {
            if ((from lid in GetLeden()
                 where lid.Lidnummer == lidnummer
                 select lid).Count() != 0)
                return true;
            else
                return false;
        }
    }
}
