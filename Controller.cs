using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Server;

[ApiController]
[Route("api/[controller]")]
public class Controller : ControllerBase {

    private readonly DatabaseService _databaseService;
    private readonly ILogger<Controller> _logger;

    public Controller(DatabaseService databaseService, ILogger<Controller> logger) {
        _databaseService = databaseService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetArtWorks() {
        try {
            _logger.LogInformation("Getting all artworks");
            var artWorks = await _databaseService.ArtWorks.Find(new BsonDocument()).ToListAsync();

            return Ok(artWorks);
        }
        catch(Exception e) {
            _logger.LogError(e, "Error getting all artworks");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtWorkById(int id) {
        try {
            _logger.LogInformation($"Getting artwork with id {id}");
            var artWork = await _databaseService.ArtWorks.Find(artWork => artWork.Id == id).FirstOrDefaultAsync();
            if (artWork == null)
            {
                return NotFound();
            }

            return Ok(artWork);
        }
        catch(Exception e) {
            _logger.LogError(e, $"Error getting artwork with id {id}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateArtWork(ArtWork artWork) {
        try {
            _logger.LogInformation($"Creating new artwork with id {artWork.Id}");
            await _databaseService.ArtWorks.InsertOneAsync(artWork);

            return CreatedAtAction(nameof(GetArtWorkById), new { id = artWork.Id }, artWork);
        }
        catch(Exception e) {
            _logger.LogError(e, $"Error creating artwork with id {artWork.Id}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtWork(int id, ArtWork artWork) {
        try {
            _logger.LogInformation($"Updating artwork with id {id}");
            await _databaseService.ArtWorks.ReplaceOneAsync(artWork => artWork.Id == id, artWork);

            return NoContent();
        }
        catch(Exception e) {
            _logger.LogError(e, $"Error updating artwork with id {id}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtWork(int id) {
        try {
            _logger.LogInformation($"Deleting artwork with id {id}");
            var artWork = await _databaseService.ArtWorks.FindOneAndDeleteAsync(artWork => artWork.Id == id);
            if (artWork == null)
            {
                return NotFound();
            }

            return Ok(artWork);
        }
        catch(Exception e) {
            _logger.LogError(e, $"Error deleting artwork with id {id}");
            return StatusCode(500, "Internal server error");
        }
    }
}