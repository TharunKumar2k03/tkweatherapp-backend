using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ServerWeather.Data;
using System.Net.Mail;
using System.Net;
using ServerWeather.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("https://tkweatherapp-production.up.railway.app")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .WithExposedHeaders("*")
               .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddControllers();

// Add MongoDB configuration
builder.Services.Configure<MongoDBSettings>(options =>
{
    options.ConnectionString = "mongodb+srv://mrtharunkumar2k03:qwertyuiop@weatherapp.detoca2.mongodb.net/?retryWrites=true&w=majority&appName=weatherapp";
    options.DatabaseName = "Weatherdb";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comment out HTTPS redirection since we're not using HTTPS in development
//app.UseHttpsRedirection();

// Ensure correct middleware order
app.UseRouting();

// Add headers to disable caching
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

// Move CORS middleware before routing
app.UseCors("AllowAll"); // Move this line before UseRouting
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
