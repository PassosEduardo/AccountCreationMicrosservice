using MongoDB.Driver;

namespace AccountService.Infrastructure.MongoInfrastrcuture;

public interface IMongoRepository
{
    IMongoDatabase? Database { get; }
}
