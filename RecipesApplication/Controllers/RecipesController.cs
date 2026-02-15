using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RecipesApplication.Models;

namespace RecipesApplication.Controllers
{
    public class RecipesController : Controller
    {
        public IActionResult Index()
        {
            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Recipe>("recipes");

            List<Recipe> recipes = collection.Find(r => true).ToList();

            return View(recipes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Recipe recipe)
        {
            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Recipe>("recipes");
            collection.InsertOne(recipe);

            return Redirect("/Recipes");
        }

        public IActionResult Show(string Id)
        {
            ObjectId recipeId =new ObjectId(Id);

            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Recipe>("recipes");

            Recipe recipe = collection.Find(r => r.Id == recipeId).FirstOrDefault();
            
            return View(recipe);
        }

        public IActionResult Edit(string Id)
        {
            ObjectId recipeId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Recipe>("recipes");

            Recipe recipe = collection.Find(r => r.Id == recipeId).FirstOrDefault();

            return View(recipe);
        }

        [HttpPost]
        public IActionResult Edit(string Id, Recipe recipe)
        {
            ObjectId recipeId = new ObjectId(Id);

            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Recipe>("recipes");

            recipe.Id =recipeId;
            collection.ReplaceOne(r => r.Id == recipeId, recipe);

            return Redirect("/Recipes");
        }

        [HttpPost]
        public IActionResult Delete(string Id)
        {
            ObjectId recipeId = new ObjectId(Id);
            MongoClient dbClient = new MongoClient();

            var database = dbClient.GetDatabase("recipes_application");
            var collection = database.GetCollection<Recipe>("recipes");

            collection.DeleteOne(r => r.Id == recipeId);

            return Redirect("/Recipes");
        }
    }


    }
