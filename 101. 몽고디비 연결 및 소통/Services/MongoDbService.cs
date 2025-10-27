using Microsoft.Extensions.Options;
using Mongo.DbSettings;
using Mongo.Models;
using MongoDB.Driver;

namespace Mongo.Services;

public class MongoDbService
{
    private readonly IMongoCollection<User> _service;

    public MongoDbService(IOptions<MongoDbSettings> options)
    {
        _service = new MongoClient(options.Value.ConnectionString)
            .GetDatabase(options.Value.Database)
            .GetCollection<User>(options.Value.Collection);
        // 위와 아래와 같은 코드
        MongoClient client = new MongoClient(options.Value.ConnectionString);
        IMongoDatabase db = client.GetDatabase(options.Value.Database);
        _service = db.GetCollection<User>(options.Value.Collection);
    }

    public async Task<List<User>> GetAllAsync() => await _service.Find(_ => true).ToListAsync();

    public async Task<User> GetAsync(string userId) => await _service.Find(u => u.UserId == userId).FirstAsync();

    public async Task PostAsync(User user) => await _service.InsertOneAsync(user);

    public async Task DeleteAsync(User user) => await _service.DeleteOneAsync(u => u.Id==user.Id);

    //public async Task UpdateAsync(User user) => await _service.UpdateOneAsync(u => u.UserId == user.UserId, user);
}