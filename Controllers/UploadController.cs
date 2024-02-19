using assignment_3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UploadController : Controller
{
    private readonly IMongoDatabase _database;
    private readonly GridFSBucket _gridFsBucket;

    public UploadController(IMongoDatabase database)
    {
        _database = database;
        _gridFsBucket = new GridFSBucket(database);
    }

    public IActionResult Index()
    {
        var data = _database.GetCollection<UploadedData>("uploadedData").Find(_ => true).ToList();
        return View(data);
    }

    [HttpGet]
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, string text)
    {
        if (file != null && file.Length > 0)
        {
            using (var stream = file.OpenReadStream())
            {
                var fileId = await _gridFsBucket.UploadFromStreamAsync(file.FileName, stream);

                var data = new UploadedData
                {
                    Text = text,
                    ImageId = fileId
                };

                var collection = _database.GetCollection<UploadedData>("uploadedData");
                await collection.InsertOneAsync(data);
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult GetImage(string id)
    {
        var imageId = new ObjectId(id);
        var imageStream = _gridFsBucket.OpenDownloadStream(imageId);
        return File(imageStream, "image/jpeg"); // Adjust MIME type as per your requirements
    }
}
