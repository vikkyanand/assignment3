using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// MongoDB Configuration
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var connectionString = "mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+2.1.3";
    return new MongoClient(connectionString);
});
builder.Services.AddSingleton(serviceProvider =>
{
    var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
    var databaseName = "userDataBase";
    return mongoClient.GetDatabase(databaseName);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Upload/Upload");
        return;
    }

    await next();
});

// Middleware to log user entry
app.Use(async (context, next) =>
{
    var database = context.RequestServices.GetRequiredService<IMongoDatabase>();
    var collection = database.GetCollection<UserEntry>("userEntries");

    var entry = new UserEntry
    {
        DateTime = DateTime.UtcNow,
        IpAddress = context.Connection.RemoteIpAddress.ToString()
    };

    await collection.InsertOneAsync(entry);
    await next();
});

app.Run();
