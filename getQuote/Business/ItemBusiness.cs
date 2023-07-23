using getQuote.Models;

namespace getQuote
{
    public class ItemBusiness
    {
        private readonly ItemRepository _repository;

        public ItemBusiness(ItemRepository itemRepository)
        {
            _repository = itemRepository;
        }

        public async Task<IEnumerable<ItemModel>> GetItems()
        {
            IEnumerable<ItemModel> items = await _repository.GetAllAsync();
            return items.OrderByDescending(i => i.ItemId);
        }

        public async Task AddAsync(ItemModel item)
        {
            await _repository.AddAsync(item);
        }

        public async Task<ItemModel> GetByIdAsync(int itemId)
        {
            return await _repository.GetByIdAsync(itemId);
        }

        public async Task UpdateAsync(ItemModel item)
        {
            ItemModel existentItem = await _repository.GetByIdAsync(item.ItemId);

            if (existentItem != null)
            {
                updateExistentItemInformation(existentItem, item);
            }

            await _repository.UpdateAsync(existentItem);
        }

        private void updateExistentItemInformation(ItemModel existentItem, ItemModel itemToUpdate)
        {
            existentItem.ItemName = itemToUpdate.ItemName;
            existentItem.Value = itemToUpdate.Value;
            existentItem.Description = itemToUpdate.Description;

            if (itemToUpdate.ItemImageList != null)
            {
                existentItem.ItemImageList?.AddRange(itemToUpdate.ItemImageList);
            }

            existentItem.SetDefaultImage(itemToUpdate.DefaultImage);

            if (itemToUpdate.IdImagesToDelete?.Count > 0)
            {
                deleteItemImage(existentItem, itemToUpdate.IdImagesToDelete);
            }
            ;
        }

        private void deleteItemImage(ItemModel existentItem, List<int> idImagesToDelete)
        {
            foreach (int idItemImage in idImagesToDelete)
            {
                ItemImageModel? itemImage = existentItem.ItemImageList?.Find(
                    im => im.ItemImageId == idItemImage
                );
                if (itemImage != null)
                {
                    existentItem.ItemImageList?.Remove(itemImage);
                }
                ;
            }
        }

        public async Task RemoveAsync(int itemId)
        {
            ItemModel item = await _repository.GetByIdAsync(itemId);
            if (item == null)
            {
                return;
            }
            ;

            if (item.ItemImageList != null)
            {
                foreach (ItemImageModel image in item.ItemImageList)
                {
                    ItemImageModel itemImage = await _repository.GetItemImageByIdAsync(
                        image.ItemImageId
                    );
                    await _repository.RemoveItemImageAsync(itemImage);
                }
            }

            await _repository.RemoveAsync(item);
        }
    }
}
