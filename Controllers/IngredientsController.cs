using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RecipesApplication.Models;
using RecipesApplication.Services;

namespace RecipesApplication.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public IngredientsController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            var collection = _mongoDbService.GetIngredientsCollection();
            var ingredients = await collection.Find(FilterDefinition<Ingredient>.Empty).ToListAsync();
            return View(ingredients);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var collection = _mongoDbService.GetIngredientsCollection();
            var ingredient = await collection.Find(i => i.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (ingredient == null) return NotFound();
            return View(ingredient);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Taste")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                var collection = _mongoDbService.GetIngredientsCollection();
                await collection.InsertOneAsync(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var collection = _mongoDbService.GetIngredientsCollection();
            var ingredient = await collection.Find(i => i.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (ingredient == null) return NotFound();
            return View(ingredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Taste")] Ingredient ingredient)
        {
            if (id != ingredient.Id.ToString()) return NotFound();
            if (ModelState.IsValid)
            {
                var collection = _mongoDbService.GetIngredientsCollection();
                await collection.ReplaceOneAsync(i => i.Id == ingredient.Id, ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var collection = _mongoDbService.GetIngredientsCollection();
            var ingredient = await collection.Find(i => i.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (ingredient == null) return NotFound();
            return View(ingredient);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var collection = _mongoDbService.GetIngredientsCollection();
            await collection.DeleteOneAsync(i => i.Id == ObjectId.Parse(id));
            return RedirectToAction(nameof(Index));
        }
    }
}