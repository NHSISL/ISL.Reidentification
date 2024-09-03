using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ISL.Reidentification.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder =
                WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddDbContext<StorageBroker>();
            AddProviders(builder.Services, configuration);
            AddBrokers(builder.Services, configuration);
            AddFoundationServices(builder.Services, configuration);
            AddProcessingServices(builder.Services, configuration);
            AddOrchestrationServices(builder.Services, configuration);
            AddCoordinationServices(builder.Services, configuration);

            WebApplication webApplication =
                builder.Build();

            if (webApplication.Environment.IsDevelopment())
            {
                webApplication.UseSwagger();
                webApplication.UseSwaggerUI();
            }

            webApplication.UseHttpsRedirection();
            webApplication.UseAuthorization();
            webApplication.MapControllers();
            webApplication.Run();
        }

        private static void AddProviders(IServiceCollection services, IConfiguration configuration)
        { }

        private static void AddBrokers(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddTransient<IStorageBroker, StorageBroker>();
        }

        private static void AddFoundationServices(IServiceCollection services, IConfiguration configuration)
        { }

        private static void AddProcessingServices(IServiceCollection services, IConfiguration configuration)
        { }

        private static void AddOrchestrationServices(IServiceCollection services, IConfiguration configuration)
        { }

        private static void AddCoordinationServices(IServiceCollection services, IConfiguration configuration)
        { }
    }
}
