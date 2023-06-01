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
        connectionString = "mongodb+srv://" + 
            Environment.GetEnvironmentVariable("USER") + ":" + 
            Environment.GetEnvironmentVariable("PASSWORD") + "@" +
            Environment.GetEnvironmentVariable("CLUSTER") + ".mongodb.net/?retryWrites=true&w=majority&connectTimeoutMS=120000&socketTimeoutMS=60000";
    }

    public void ConfigureServices(IServiceCollection services) {

        if (string.IsNullOrEmpty(connectionString)) {
            throw new InvalidOperationException("Missing connection string in environment variables.");
        }

        services.AddSingleton<DatabaseService>(provider =>
            new DatabaseService(connectionString, "GrafittiWallDB"));

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }
        else {
            app.UseHttpsRedirection();
            app.UseHsts();
        }
        
        //app.UseAuthorization(); //can be fixed later
        app.UseRouting();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}