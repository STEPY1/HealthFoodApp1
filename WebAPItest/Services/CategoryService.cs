using MongoDB.Driver;
using WebAPItest.Models;

namespace WebAPItest.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        public CategoryService(IMongoDatabase database)
        {
            _categoryCollection = database.GetCollection<Category>("Categories");
        }

        public async Task<List<Category>> GetLimitAsync(int limit) =>
            await _categoryCollection.Find(_ => true).Limit(limit).ToListAsync();

        public async Task<List<Category>> GetAllAsync() =>
            await _categoryCollection.Find(_ => true).ToListAsync();

        public async Task<Category> GetByIdAsync(string id) =>
            await _categoryCollection.Find(f => f.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Category item) =>
            await _categoryCollection.InsertOneAsync(item);
        public async Task UpdateAsync(string id, Category item) =>
            await _categoryCollection.ReplaceOneAsync(f => f.Id == id, item);
        public async Task RemoveAsync(string id) =>
            await _categoryCollection.DeleteOneAsync(f => f.Id == id);
    }
}
