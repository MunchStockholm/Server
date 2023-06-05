using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Server;

public class Startup {

    public IConfiguration Configuration { get; }
    private readonly string connectionString;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        string user = Environment.GetEnvironmentVariable("USER")!;
        string password = Environment.GetEnvironmentVariable("PASSWORD")!;
        string cluster = Environment.GetEnvironmentVariable("CLUSTER")!;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(cluster)) {
            throw new InvalidOperationException("One of the required environment variables is missing.");
        }

        connectionString = $"mongodb+srv://{user}:{password}@{cluster}.mongodb.net/?retryWrites=true&w=majority&connectTimeoutMS=120000&socketTimeoutMS=60000";
    }

    public void ConfigureServices(IServiceCollection services) {

        if (string.IsNullOrEmpty(connectionString)) {
            throw new InvalidOperationException("Missing connection string in environment variables.");
        }

        services.AddSingleton<DatabaseService>(provider =>
            new DatabaseService(connectionString, "GrafittiWallDB"));

        services.AddControllers();

        services.AddCors(options => {
            options.AddDefaultPolicy(builder => {
                // Replace "http://localhost:3000" with the correct origin of your React app
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseHttpsRedirection();
            app.UseHsts();
        }
        
        app.UseCors();
        app.UseRouting();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}