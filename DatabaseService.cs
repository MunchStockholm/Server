using MongoDB.Driver;

namespace Server;

public class DatabaseService {
    private readonly IMongoDatabase _database;

    public DatabaseService(string connectionString, string databaseName) {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<ArtWork> ArtWorks => _database.GetCollection<ArtWork>("ArtWork");
}