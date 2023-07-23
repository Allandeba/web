using System.Linq.Expressions;
using getQuote.DAO;
using getQuote.Models;
using Microsoft.EntityFrameworkCore;

namespace getQuote
{
    public class ItemRepository : IRepository<ItemModel>
    {
        private readonly ApplicationDBContext _context;

        public ItemRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ItemModel> GetByIdAsync(int itemId)
        {
            return await _context.Item
                .Include(i => i.ItemImageList)
                .FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        public async Task<ItemImageModel> GetItemImageByIdAsync(int itemImageId)
        {
            return await _context.ItemImage.FindAsync(itemImageId);
        }

        public async Task<IEnumerable<ItemModel>> GetAllAsync()
        {
            return await _context.Item.ToListAsync();
        }

        public async Task<IEnumerable<ItemModel>> FindAsync(Expression<Func<ItemModel, bool>> where)
        {
            return await _context.Item.Where(where).ToListAsync();
        }

        public async Task AddAsync(ItemModel item)
        {
            await _context.Item.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ItemModel item)
        {
            _context.Item.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(ItemModel item)
        {
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemImageAsync(ItemImageModel itemImage)
        {
            _context.ItemImage.Remove(itemImage);
            await _context.SaveChangesAsync();
        }
    }
}
