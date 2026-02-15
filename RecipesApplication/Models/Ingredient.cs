using MongoDB.Bson;

namespace RecipesApplication.Models
{
    public class Ingredient
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Taste { get; set; }
    }
}
