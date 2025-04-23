using MongoDB.Driver;
using WebAPItest.Models;


namespace WebAPItest.Services
{
    public class RestaurantService
    {
        private readonly IMongoCollection<Restaurant> _restaurantColletions;
        public RestaurantService(IMongoDatabase database)
        {
            _restaurantColletions = database.GetCollection<Restaurant>("Restaurants");
        }
        public async Task<List<Restaurant>> GetAllAsync() =>
            await _restaurantColletions.Find(_ => true).ToListAsync();
        public async Task<Restaurant> GetByIdAsync(string id) =>
            await _restaurantColletions.Find(f => f.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Restaurant item) =>
            await _restaurantColletions.InsertOneAsync(item);
        public async Task UpdateAsync(string id, Restaurant item) =>
            await _restaurantColletions.ReplaceOneAsync(f => f.Id == id, item);
        public async Task RemoveAsync(string id) =>
            await _restaurantColletions.DeleteOneAsync(f => f.Id == id);
        public async Task<List<Restaurant>> GetByLimitAsync(int limit)
        {

            return await _restaurantColletions.Find(_ => true).Limit(limit).ToListAsync();
        }
    }
}
