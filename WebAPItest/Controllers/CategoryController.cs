using Microsoft.AspNetCore.Mvc;
using WebAPItest.Models;
using WebAPItest.Services;

namespace WebAPItest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _CategoriesService;
        public CategoryController(CategoryService CategoriesService)
        {
            _CategoriesService = CategoriesService;
        }

        [HttpGet("Limit/{limit}")]
        public async Task<IActionResult> GetLimit(int limit)
        {
            var result = await _CategoriesService.GetLimitAsync(limit);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await _CategoriesService.GetAllAsync());

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _CategoriesService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            await _CategoriesService.RemoveAsync(id);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, Category item)
        {
            var result = await _CategoriesService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            item.Id = id;
            await _CategoriesService.UpdateAsync(id, item);
            return Ok(item);
        }

        [HttpGet("Find")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await _CategoriesService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category item)
        {
            await _CategoriesService.CreateAsync(item);
            return Ok(item);
        }
    }
}
