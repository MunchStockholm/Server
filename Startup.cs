using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Server;

public class Startup {

    public IConfiguration Configuration { get; }
    private readonly string connectionString;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        connectionString = "mongodb+srv://" + 
            Environment.GetEnvironmentVariable("USER") + ":" + 
            Environment.GetEnvironmentVariable("PASSWORD") + "@" +
            Environment.GetEnvironmentVariable("CLUSTER") + ".mongodb.net/";
    }

    public void ConfigureServices(IServiceCollection services) {

        //string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString)) {
            throw new InvalidOperationException("Missing connection string in environment variables.");
        }

        services.AddScoped<DatabaseService>(provider =>
            new DatabaseService(connectionString, "GrafittiWallDB"));

        services.AddControllers();
        services.AddSingleton<DatabaseService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        app.UseHttpsRedirection();
        app.UseRouting();

        //app.UseAuthorization(); //can be fixed later

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}