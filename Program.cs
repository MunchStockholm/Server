using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server;
public class Program {
    public static void Main(string[] args) {

        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
    
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<Program>>();

        try {
            logger.LogInformation("Getting database service...");
            var dbService = services.GetRequiredService<DatabaseService>();
            logger.LogInformation("Database service obtained.");


            // Tester å laste ned et bilde fra nettet og lagre det i databasen
            logger.LogInformation("Downloading artwork...");
            string imageUrlString = "https://www.munchmuseet.no/globalassets/kunstverk/madonna_crop.jpg";
            byte[] imageBytes = DownloadImageToByteArrayAsync(imageUrlString).Result;
            
            var artwork = new ArtWork
            {
                ImageBytes = imageBytes,
                ImageUrl = imageUrlString,
                IsFeatured = true,
                CreatedDate = DateTime.Now
            };
            
            logger.LogInformation("Inserting artwork into the database...");
            dbService.ArtWorks.InsertOne(artwork);
            logger.LogInformation("Artwork downloaded and saved to the database.");
        }
        catch (Exception e) {
            logger.LogError(e, "An error occurred creating the DB.");
        }
        
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
    });

    public static async Task<byte[]> DownloadImageToByteArrayAsync(string imageUrl) {
        using HttpClient httpClient = new HttpClient();
        byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

        return imageBytes;
    }
}