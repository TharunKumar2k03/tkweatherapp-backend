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
        builder.WithOrigins("http://localhost:5084")  // Your Blazor app URL
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
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

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
