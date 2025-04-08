using MongoDB.Driver;
using ServerWeather.Entity;
using Microsoft.Extensions.Options;
using ServerWeather.Models;  // Updated namespace

namespace ServerWeather.Data
{
    public class AppDbContext
    {
        private readonly IMongoDatabase _database;

        public AppDbContext(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var Client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            _database = Client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        }

        public IMongoCollection<WeatherForecast> WeatherForecast =>
             _database.GetCollection<WeatherForecast>("WeatherForecast");

        public IMongoCollection<UserFavorites> FavoriteCity =>
            _database.GetCollection<UserFavorites>("Favourites");
    }
}
