using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Server;
public class Program {
    public static void Main(string[] args) {
        //CreateHostBuilder(args).Build().Run();
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
    
        var services = scope.ServiceProvider;

        try {
            var dbService = services.GetRequiredService<DatabaseService>();

            // Tester å laste ned et bilde fra nettet og lagre det i databasen
            string imageUrlString = "https://www.munchmuseet.no/globalassets/kunstverk/madonna_crop.jpg";
            byte[] imageBytes = DownloadImageToByteArrayAsync(imageUrlString).Result;
            
            var artwork = new ArtWork
            {
                ImageBytes = imageBytes,
                ImageUrl = imageUrlString,
                IsFeatured = true,
                CreatedDate = DateTime.Now
            };
            
            dbService.ArtWorks.InsertOne(artwork);
        }
        catch (Exception e) {
            var logger = services.GetRequiredService<ILogger<Program>>();
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