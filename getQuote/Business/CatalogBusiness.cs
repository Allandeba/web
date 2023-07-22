using getQuote.Models;

namespace getQuote
{
    public class CatalogBusiness
    {
        private readonly CatalogRepository _repository;

        public CatalogBusiness(CatalogRepository catalogRepository)
        {
            _repository = catalogRepository;
        }

        public IEnumerable<ItemModel> GetItems()
        {
            return _repository.GetItems();
        }
    }
}
