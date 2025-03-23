using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;  // Lägg till denna rad

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RestaurantsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Restaurants
        [HttpGet]
        public async Task<IActionResult> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.ToListAsync();  // Nu tillgänglig
            return Ok(restaurants);
        }

        // POST: api/Restaurants
        [HttpPost]
        public async Task<IActionResult> CreateRestaurant([FromBody] Restaurant restaurant)
        {
            if (restaurant == null)
            {
                return BadRequest();
            }

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRestaurants", new { id = restaurant.Id }, restaurant);
        }
    }
}
