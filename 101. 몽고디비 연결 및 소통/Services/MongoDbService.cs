using Microsoft.Extensions.Options;
using Mongo.DbSettings;
using Mongo.Models;
using MongoDB.Driver;

namespace Mongo.Services;

public class MongoDbService
{
    private readonly IMongoCollection<User> _service;

    private MongoDbService(IOptions<MongoDbSettings> options)
    {
        _service = new MongoClient(options.Value.ConnectionString)
            .GetDatabase(options.Value.Database)
            .GetCollection<User>(options.Value.Collection);
        // 위와 아래와 같은 코드
        MongoClient client = new MongoClient(options.Value.ConnectionString);
        IMongoDatabase db = client.GetDatabase(options.Value.Database);
        _service = db.GetCollection<User>(options.Value.Collection);

    }

    // UserId 속성 인덱스 설정
    public static async Task<MongoDbService> CreateAsync(IOptions<MongoDbSettings> options)
    {
        MongoDbService db = new(options);

        var index = Builders<User>.IndexKeys.Ascending(u => u.UserId);
        CreateIndexOptions<User> idxOption = new() { Unique = true };
        await db._service.Indexes.CreateOneAsync(new CreateIndexModel<User>(index, idxOption));

        return db;
    }


    public async Task<List<User>> GetAllAsync() => await _service.Find(_ => true).ToListAsync();

    public async Task<User> GetAsync(string userId) => await _service.Find(u => u.UserId == userId).FirstAsync();

    public async Task PostAsync(User user) => await _service.InsertOneAsync(user);

    public async Task PutAsync(User user) => await _service.ReplaceOneAsync(u => u.Id == user.Id, user);

    public async Task DeleteAsync(User user) => await _service.DeleteOneAsync(u => u.Id==user.Id);

}