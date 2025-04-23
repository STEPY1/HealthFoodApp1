using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IMongoDatabase _database;

        public TestController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpGet("ping")]
        public async Task<IActionResult> PingMongo()
        {
            try
            {
                var result = await _database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
                return Ok("✅ Kết nối MongoDB thành công!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Lỗi kết nối MongoDB: {ex.Message}");
            }
        }
    }
}
