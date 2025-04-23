using Microsoft.AspNetCore.Mvc;
using WebAPItest.Models;
using WebAPItest.Services;

namespace WebAPItest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantService _restaurantService;
        public RestaurantController(RestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _restaurantService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Restaurant restaurant)
        {
            await _restaurantService.CreateAsync(restaurant);
            return Ok(restaurant);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, Restaurant restaurant)
        {
            var existingRestaurant = await _restaurantService.GetByIdAsync(id);
            if (existingRestaurant == null)
            {
                return NotFound();
            }
            restaurant.Id = id;
            await _restaurantService.UpdateAsync(id, restaurant);
            return Ok(restaurant);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _restaurantService.RemoveAsync(id);
            return NoContent();
        }
        [HttpGet("limit/{limit}")]
        public async Task<IActionResult> GetByLimit(int limit)
        {
            var restaurants = await _restaurantService.GetByLimitAsync(limit);
            if (restaurants == null || !restaurants.Any())
            {
                return NotFound();
            }
            return Ok(restaurants);
        }
    }
}
