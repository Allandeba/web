using System.Linq.Expressions;
using getQuote.DAO;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote
{
    public class ProposalHistoryRepository : IRepository<ProposalHistoryModel>
    {
        private readonly ApplicationDBContext _context;

        public ProposalHistoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        private IQueryable<ProposalHistoryModel> GetQuery(Enum[] includes)
        {
            IQueryable<ProposalHistoryModel> query = _context.ProposalHistory;
            foreach (ProposalHistoryIncludes include in includes)
            {
                switch (include)
                {
                    case ProposalHistoryIncludes.None:
                        break;

                    case ProposalHistoryIncludes.Person:
                        query = query.Include(p => p.Person);
                        break;

                    case ProposalHistoryIncludes.Proposal:
                        query = query.Include(pp => pp.Proposal);
                        break;

                    default:
                        throw new Exception("ProposalHistoryIncludes type not implemented");
                }
            }

            return query;
        }

        public async Task<IEnumerable<ProposalHistoryModel>> GetAllAsync(Enum[] includes)
        {
            IQueryable<ProposalHistoryModel> query = GetQuery(includes);
            return await query.ToListAsync();
        }

        public async Task<ProposalHistoryModel> GetByIdAsync(int proposalHistoryId, Enum[] includes)
        {
            IQueryable<ProposalHistoryModel> query = GetQuery(includes);
            return await query.FirstOrDefaultAsync(ph => ph.ProposalHistoryId == proposalHistoryId);
        }

        public async Task<IEnumerable<ProposalHistoryModel>> FindAsync(
            Expression<Func<ProposalHistoryModel, bool>> where
        )
        {
            return await _context.ProposalHistory.Where(where).ToListAsync();
        }

        public async Task AddAsync(ProposalHistoryModel proposalHistory)
        {
            await _context.ProposalHistory.AddAsync(proposalHistory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProposalHistoryModel proposalHistory)
        {
            _context.ProposalHistory.Update(proposalHistory);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ProposalHistoryModel proposalHistory)
        {
            _context.ProposalHistory.Remove(proposalHistory);
            await _context.SaveChangesAsync();
        }
    }
}
