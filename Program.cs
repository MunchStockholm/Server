using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Server;
public class Program {
    public static void Main(string[] args) {
        //CreateHostBuilder(args).Build().Run();
        var host = CreateHostBuilder(args).Build();

        using var scope = host.Services.CreateScope();
    
        var services = scope.ServiceProvider;

        try {
            var dbService = services.GetRequiredService<DatabaseService>();
            
            var artwork = new ArtWork
            {
                //Id = 1000,
                ImageUrl = "https://www.munchmuseet.no/globalassets/kunstverk/madonna_crop.jpg",
                ImageThumbnailUrl = "https://www.munchmuseet.no/globalassets/kunstverk/madonna_crop.jpg",
                IsFeatured = true,
                CreatedDate = DateTime.Now
            };
            
            dbService.ArtWorks.InsertOne(artwork);
        }
        catch (Exception ex) {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
        
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
            webBuilder.UseStartup<Startup>();
    });
}