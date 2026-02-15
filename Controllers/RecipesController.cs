using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using RecipesApplication.Models;
using RecipesApplication.Services;

namespace RecipesApplication.Controllers
{
    public class RecipesController : Controller
    {
        private readonly MongoDbService _mongoDbService;

        public RecipesController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IActionResult> Index()
        {
            var collection = _mongoDbService.GetRecipesCollection();
            var recipes = await collection.Find(FilterDefinition<Recipe>.Empty).ToListAsync();
            return View(recipes);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var recipeCollection = _mongoDbService.GetRecipesCollection();
            var recipe = await recipeCollection.Find(r => r.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (recipe == null) return NotFound();

            var ingredientCollection = _mongoDbService.GetIngredientsCollection();
            var ingredients = new List<Ingredient>();
            foreach (var ingredientId in recipe.IngredientIds)
            {
                var ingredient = await ingredientCollection.Find(i => i.Id == ingredientId).FirstOrDefaultAsync();
                if (ingredient != null) ingredients.Add(ingredient);
            }
            ViewBag.Ingredients = ingredients;
            return View(recipe);
        }

        public async Task<IActionResult> Create()
        {
            var collection = _mongoDbService.GetIngredientsCollection();
            var ingredients = await collection.Find(FilterDefinition<Ingredient>.Empty).ToListAsync();
            ViewBag.Ingredients = ingredients;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Recipe recipe, string[] selectedIngredients)
        {
            if (ModelState.IsValid)
            {
                if (selectedIngredients != null && selectedIngredients.Length > 0)
                {
                    recipe.IngredientIds = selectedIngredients.Select(id => ObjectId.Parse(id)).ToList();
                }
                var collection = _mongoDbService.GetRecipesCollection();
                await collection.InsertOneAsync(recipe);
                return RedirectToAction(nameof(Index));
            }
            var ingredientCollection = _mongoDbService.GetIngredientsCollection();
            var ingredients = await ingredientCollection.Find(FilterDefinition<Ingredient>.Empty).ToListAsync();
            ViewBag.Ingredients = ingredients;
            return View(recipe);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var recipeCollection = _mongoDbService.GetRecipesCollection();
            var recipe = await recipeCollection.Find(r => r.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (recipe == null) return NotFound();

            var ingredientCollection = _mongoDbService.GetIngredientsCollection();
            var ingredients = await ingredientCollection.Find(FilterDefinition<Ingredient>.Empty).ToListAsync();
            ViewBag.Ingredients = ingredients;
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name")] Recipe recipe, string[] selectedIngredients)
        {
            if (id != recipe.Id.ToString()) return NotFound();
            if (ModelState.IsValid)
            {
                if (selectedIngredients != null && selectedIngredients.Length > 0)
                {
                    recipe.IngredientIds = selectedIngredients.Select(ingId => ObjectId.Parse(ingId)).ToList();
                }
                else
                {
                    recipe.IngredientIds = new List<ObjectId>();
                }
                var collection = _mongoDbService.GetRecipesCollection();
                await collection.ReplaceOneAsync(r => r.Id == recipe.Id, recipe);
                return RedirectToAction(nameof(Index));
            }
            var ingredientCollection = _mongoDbService.GetIngredientsCollection();
            var ingredients = await ingredientCollection.Find(FilterDefinition<Ingredient>.Empty).ToListAsync();
            ViewBag.Ingredients = ingredients;
            return View(recipe);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var collection = _mongoDbService.GetRecipesCollection();
            var recipe = await collection.Find(r => r.Id == ObjectId.Parse(id)).FirstOrDefaultAsync();
            if (recipe == null) return NotFound();
            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var collection = _mongoDbService.GetRecipesCollection();
            await collection.DeleteOneAsync(r => r.Id == ObjectId.Parse(id));
            return RedirectToAction(nameof(Index));
        }
    }
}