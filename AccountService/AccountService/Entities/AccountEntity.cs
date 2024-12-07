using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace AccountService.Entities;

public class AccountEntity
{
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? Id {  get; set; }

    [BsonElement("email"), BsonRepresentation(BsonType.String)]
    public string Email { get; set; }

    [BsonElement("email_is_confirmed"), BsonRepresentation(BsonType.Boolean)]
    public bool EmailIsConfirmed { get; set; }

    [BsonElement("email_confirmation_token"), BsonRepresentation(BsonType.String)]
    public string? EmailToken { get; set; }

    [BsonElement("password"), BsonRepresentation(BsonType.String)]
    public string Password { get; set; }

    [BsonElement("salt"), BsonRepresentation(BsonType.String)]
    public string? Salt { get; set; }
}
