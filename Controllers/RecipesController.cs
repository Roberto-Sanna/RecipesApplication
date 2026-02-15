using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RecipesApplication.Models; // Assuming Models namespace contains Recipe model

namespace RecipesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IMongoCollection<Recipe> _recipes;

        public RecipesController(IMongoClient client)
        {
            var database = client.GetDatabase("RecipesDb");
            _recipes = database.GetCollection<Recipe>("Recipes");
        }

        // Create
        [HttpPost]
        public async Task<ActionResult<Recipe>> Create(Recipe recipe)
        {
            await _recipes.InsertOneAsync(recipe);
            return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe);
        }

        // Read All
        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetAll()
        {
            var recipes = await _recipes.Find(r => true).ToListAsync();
            return Ok(recipes);
        }

        // Read by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetById(string id)
        {
            var recipe = await _recipes.Find(r => r.Id == id).FirstOrDefaultAsync();
            if (recipe == null) return NotFound();
            return Ok(recipe);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Recipe recipeIn)
        {
            var recipe = await _recipes.Find(r => r.Id == id).FirstOrDefaultAsync();
            if (recipe == null) return NotFound();
            
            recipeIn.Id = recipe.Id; // Preserve the original ID
            await _recipes.ReplaceOneAsync(r => r.Id == id, recipeIn);
            return NoContent();
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var recipe = await _recipes.Find(r => r.Id == id).FirstOrDefaultAsync();
            if (recipe == null) return NotFound();

            await _recipes.DeleteOneAsync(r => r.Id == id);
            return NoContent();
        }
    }
}