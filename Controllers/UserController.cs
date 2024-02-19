using assign1.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;

public class UserController : Controller
{
    private static List<UserInfo> users = new List<UserInfo>();
    private readonly IMongoDatabase _database;

    public UserController(IMongoDatabase database)
    {
        _database = database;
    }

    public IActionResult Index()
    {
        return View(users);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(UserInfo userInfo)
    {
        users.Add(userInfo);

        var collection = _database.GetCollection<UserInfo>("users");
        collection.InsertOne(userInfo);

        return RedirectToAction("Index");
    }

    public IActionResult Details(string name)
    {
        var user = users.Find(u => u.Name == name);
        return View(user);
    }
}
