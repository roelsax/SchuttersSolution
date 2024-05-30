using Microsoft.EntityFrameworkCore;
using Schutters.Models;

namespace Schutters.Repositories
{
    public class SQLLedenRepository : ILedenRepository
    {
        private readonly SchuttersContext context;

        public SQLLedenRepository(SchuttersContext context) 
        {
            this.context = context;
        }

        public IEnumerable<Lid> GetAll() => context.Leden.Include("Club").AsNoTracking();

        public Lid? Get(long lidnummer) => context.Leden.Where(l => l.Lidnummer == lidnummer).Include("Club").FirstOrDefault();
        public void Add(Lid lid) 
        {
            context.Add(lid);
            context.SaveChanges();
        }
        public void Update(Lid lid)
        {
            try
            {
                context.Update(lid);

                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Delete(long lidnummer)
        {
            var lid = Get(lidnummer);
            context.Remove(lid);
            context.SaveChanges();
        }
    }
}
