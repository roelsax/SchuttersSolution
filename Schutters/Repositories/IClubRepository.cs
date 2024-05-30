using Schutters.Models;

namespace Schutters.Repositories
{
    public interface IClubRepository
    {
        public IEnumerable<Club> GetAll();
        public Club? Get(int stamnummer);
        public void Add(Club club);

        public void Update(Club club);

        public void Delete(int stamnummer);
    }
}
