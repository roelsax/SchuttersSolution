using Schutters.Repositories;
using Schutters.Models;

namespace Schutters.Services
{
    public class ClubService
    {
        private readonly IClubRepository repository;

        public ClubService(IClubRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Club> GetClubs() => repository.GetAll();

        public Club? FindClub(int stamnummer) => repository.Get(stamnummer);

        public void Create(Club club) => repository.Add(club);

        public void Update(Club club) => repository.Update(club);

        public void Remove(int stamnummer) => repository.Delete(stamnummer);
        
        public bool Bestaat(int stamnummer)
        {
            if ((from club in GetClubs()
                 where club.Stamnummer == stamnummer
                 select club).Count() != 0)
                return true;
            else
                return false;
        }
    }
}
