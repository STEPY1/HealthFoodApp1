using MongoDB.Driver;
using WebAPItest.Models;

namespace WebAPItest.Services
{
    public class FoodItemService
    {
        private readonly IMongoCollection<FoodItem> _foodService;
        public FoodItemService(IMongoDatabase database)
        {
            _foodService = database.GetCollection<FoodItem>("FoodItems");
        }
        public async Task<List<FoodItem>> GetLimitAsyns(int limit) =>
            await _foodService.Find(_ => true).Limit(limit).ToListAsync();
        public async Task<List<FoodItem>> GetAll() =>
            await _foodService.Find(_ => true).ToListAsync();
        public async Task<FoodItem> GetByIdAsync(string id) =>
            await _foodService.Find(f => f.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(FoodItem item) =>
            await _foodService.InsertOneAsync(item);
        public async Task UpdateAsync(string id, FoodItem item) =>
            await _foodService.ReplaceOneAsync(f => f.Id == id, item);
        public async Task RemoveAsync(string id) =>
            await _foodService.DeleteOneAsync(f => f.Id == id);
    }
}
