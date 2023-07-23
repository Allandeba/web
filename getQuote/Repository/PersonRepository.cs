using System.Linq.Expressions;
using getQuote.DAO;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote
{
    public class PersonRepository : IRepository<PersonModel>
    {
        private readonly ApplicationDBContext _context;

        public PersonRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<PersonModel> GetByIdAsync(int PersonId)
        {
            return await _context.Person
                .Include(c => c.Contact)
                .Include(d => d.Document)
                .FirstOrDefaultAsync(p => p.PersonId == PersonId);
        }

        public async Task<IEnumerable<PersonModel>> GetAllAsync()
        {
            return await _context.Person.ToListAsync();
        }

        public async Task<IEnumerable<PersonModel>> FindAsync(
            Expression<Func<PersonModel, bool>> where
        )
        {
            return await _context.Person.Where(where).ToListAsync();
        }

        public async Task AddAsync(PersonModel person)
        {
            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PersonModel person)
        {
            _context.Person.Update(person);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(PersonModel person)
        {
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}
