using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

public class HomeController : Controller
{
    private readonly IMongoDatabase _database;

    public HomeController(IMongoDatabase database)
    {
        _database = database;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        var collection = _database.GetCollection<UserEntry>("userEntries");
        var latestEntry = collection.Find(new BsonDocument()).SortByDescending(e => e.DateTime).Limit(1).FirstOrDefault();
        return View(latestEntry);
    }
}
