using MongoDB.Driver;

namespace AccountService.Infrastructure.MongoInfrastrcuture;

public class MongoRepository : IMongoRepository
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase? _database;
    public MongoRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetConnectionString("DbConnection");
        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);
        _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase? Database => _database;
}
