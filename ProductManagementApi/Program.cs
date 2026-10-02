using ECommerceAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using ProductManagementApi.Data;
using ProductManagementApi.Extensions;
using ProductManagementApi.Repositories;
using ProductManagementApi.Services;
using Serilog;

namespace ProductManagementApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Clear default logging providers
            builder.Logging.ClearProviders();

            // Add Console logging provider
            builder.Logging.AddConsole();

            // Add Debug logging provider
            builder.Logging.AddDebug();

            // Add controller support to the dependency injection container
            builder.Services.AddControllers()
             .AddJsonOptions(options =>
             {
                 // Disable camelCase so JSON property names remain the same as C# property names
                 options.JsonSerializerOptions.PropertyNamingPolicy = null;
             });

            builder.Services.AddEndpointsApiExplorer();
            // Register OpenAPI support.
            // Microsoft.AspNetCore.OpenApi generates the OpenAPI document
            // that will later be displayed through Swagger UI.
            builder.Services.AddSwaggerDocumentation();


            // Configure Serilog as the application's logging provider.
            // Serilog configuration is loaded from appsettings.json
            // through the AddSerilogLogging extension method.
            builder.AddSerilogLogging();


            // Register ApplicationDbContext and configure it to use SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register repository and service classes for dependency injection
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            var app = builder.Build();

            // Enable Swagger middleware for all environment
            //app.UseSwagger();
            app.UseSwaggerDocumentation();

            // Adds ASP.NET Core's TraceIdentifier to Serilog LogContext.
            // All log statements generated while processing the same HTTP
            // request can therefore contain the same TraceIdentifier.
            app.UseTraceIdentifierLogging();

            // Logs one structured summary entry for every HTTP request.
            // Typical information includes request path, status code, and execution time.
            app.UseSerilogRequestLogging();


            // Redirect all HTTP requests to HTTPS
            app.UseHttpsRedirection();

            // Adds authorization middleware to the request pipeline
            app.UseAuthorization();

            // Maps controller endpoints to incoming HTTP requests
            app.MapControllers();

            // Starts the application
            app.Run();
        }
    }
}

