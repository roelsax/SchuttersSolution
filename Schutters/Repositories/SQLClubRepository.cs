using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Schutters.Models;

namespace Schutters.Repositories
{
    public class SQLClubRepository : IClubRepository
    {
        private readonly SchuttersContext context;

        public SQLClubRepository(SchuttersContext context)
        {
            this.context = context;
        }

        public IEnumerable<Club> GetAll() => context.Clubs.AsNoTracking();

        public Club? Get(int stamnummer) {
            var club = context.Clubs.Where(c => c.Stamnummer == stamnummer).FirstOrDefault();
                return club;
                }

        public void Add(Club club) 
        { 
            context.Add(club);
            
            context.SaveChanges();
        }

        public void Update(Club club)
        {
            try
            {
                context.Update(club);

                context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
        }

        public void Delete(int stamnummer) 
        {
            var club = Get(stamnummer);
            context.Remove(club);
            context.SaveChanges();
        }
    }
}
