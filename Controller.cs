using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Database;

public class Controller {

    private readonly DatabaseService _databaseService;

    public Controller(DatabaseService databaseService) {
        _databaseService = databaseService;
    }

    public async Task<List<ArtWork>> GetArtWorks() {
        var artWorks = await _databaseService.ArtWorks.Find(new BsonDocument()).ToListAsync();
        return artWorks;
    }

    public async Task<ArtWork> GetArtWorkById(int id) {
        var artWork = await _databaseService.ArtWorks.Find(artWork => artWork.Id == id).FirstOrDefaultAsync();
        return artWork;
    }

    public async Task<ArtWork> CreateArtWork(ArtWork artWork) {
        await _databaseService.ArtWorks.InsertOneAsync(artWork);
        return artWork;
    }

    public async Task<ArtWork> UpdateArtWork(int id, ArtWork artWork) {
        await _databaseService.ArtWorks.ReplaceOneAsync(artWork => artWork.Id == id, artWork);
        return artWork;
    }

    public async Task<ArtWork> DeleteArtWork(int id) {
        var artWork = await _databaseService.ArtWorks.FindOneAndDeleteAsync(artWork => artWork.Id == id);
        return artWork;
    }

    public async Task<ArtWork> DeleteArtWork(ArtWork artWork) {
        var deletedArtWork = await _databaseService.ArtWorks.FindOneAndDeleteAsync(artWork => artWork.Id == artWork.Id);
        return deletedArtWork;
    }

}