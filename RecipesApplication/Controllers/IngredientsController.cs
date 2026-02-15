using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesApplication.Models;

namespace RecipesApplication.Controllers
{
    public class IngredientsController : Controller
    {
        public IActionResult Index()
        {
            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Ingredient>("ingredients");

            List<Ingredient> ingredients = collection.Find(i => true).ToList();

            return View(ingredients);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Ingredient ingredient)
        {
            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Ingredient>("ingredients");
            collection.InsertOne(ingredient);

            return Redirect("/Ingredients");
        }

        public IActionResult Show(string Id)
        {
            ObjectId ingredientId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Ingredient>("ingredients");

            Ingredient ingredient = collection.Find(i => i.Id == ingredientId).FirstOrDefault();

            return View(ingredient);
        }

        public IActionResult Edit(string Id)
        {
            ObjectId ingredientId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Ingredient>("ingredients");

            Ingredient ingredient = collection.Find(i => i.Id == ingredientId).FirstOrDefault();

            return View(ingredient);
        }

        [HttpPost]
        public IActionResult Edit(string Id, Ingredient ingredient)
        {
            ObjectId ingredientId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Ingredient>("ingredients");

            ingredient.Id = ingredientId;
            collection.ReplaceOne(i => i.Id == ingredientId, ingredient);

            return Redirect("/Ingredients");
        }


        [HttpPost]
        public IActionResult Delete(string Id)
        {
            ObjectId ingredientId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Ingredient>("ingredients");

            collection.DeleteOne(i => i.Id == ingredientId);

            return Redirect("/Ingredients");
        }
    }
}
    