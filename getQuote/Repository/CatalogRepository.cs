using getQuote.DAO;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote
{
    public class CatalogRepository
    {
        private readonly ApplicationDBContext _context;

        public CatalogRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public IEnumerable<ItemModel> GetItems()
        {
            return _context.Item
                .Include(i => i.ItemImageList)
                .OrderByDescending(a => a.ItemImageList.Count > 1)
                .ToList();
        }
    }
}
