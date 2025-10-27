using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Mongo.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("id")]
    public string Id { get; set; } = null!;

    [BsonElement("name")]
    public string Username { get; set; } = null!;

    [BsonElement("user_id")]
    public string UserId { get; set; } = null!;

    [BsonElement("password")]
    public string Password { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;
}