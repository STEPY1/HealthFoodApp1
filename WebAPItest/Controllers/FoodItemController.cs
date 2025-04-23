using Microsoft.AspNetCore.Mvc;
using WebAPItest.Models;
using WebAPItest.Services;
namespace WebAPItest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodItemController : Controller
    {
        private readonly FoodItemService _foodService;
        public FoodItemController(FoodItemService foodController)
        {
            _foodService = foodController;
        }
        [HttpGet("Limit/{limit}")]
        public async Task<IActionResult> GetLimit(int limit)
        {
            try
            {
                var foodItems = await _foodService.GetLimitAsyns(limit);
                if (foodItems == null)
                {
                    return NotFound();
                }
                return Ok(foodItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var foodItems = await _foodService.GetAll();
                return Ok(foodItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(FoodItem foodItem)
        {
            await _foodService.CreateAsync(foodItem);
            return Ok(foodItem);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(string id)
        {
            try
            {
                var foodItem = await _foodService.GetByIdAsync(id);
                if (foodItem == null)
                {
                    return NotFound();
                }
                await _foodService.RemoveAsync(id);
                return Ok(foodItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, FoodItem foodItem)
        {
            var existingFoodItem = await _foodService.GetByIdAsync(id);
            if (existingFoodItem == null)
            {
                return NotFound();
            }
            foodItem.Id = id;
            await _foodService.UpdateAsync(id, foodItem);
            return Ok(foodItem);
        }
    }
}
