// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Identifiers;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Services.Foundations.Lookups;
using ISL.ReIdentification.Core.Services.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.UserAccesses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

namespace ISL.ReIdentification.Configurations.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder =
                WebApplication.CreateBuilder(args);

            // Add services to the container.
            var azureAdOptions = builder.Configuration.GetSection("AzureAd");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(azureAdOptions);

            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<ReIdentificationStorageBroker>();
            builder.Services.AddDbContext<OdsStorageBroker>();
            builder.Services.AddHttpContextAccessor();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            AddProviders(builder.Services);
            AddBrokers(builder.Services);
            AddFoundationServices(builder.Services);
            AddProcessingServices(builder.Services);
            AddOrchestrationServices(builder.Services);
            AddCoordinationServices(builder.Services);

            // Register IConfiguration to be available for dependency injection
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            var app = builder.Build();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers().WithOpenApi();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void AddProviders(IServiceCollection services)
        { }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<IIdentifierBroker, IdentifierBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<IOdsStorageBroker, OdsStorageBroker>();
            services.AddTransient<IReIdentificationStorageBroker, ReIdentificationStorageBroker>();
        }

        private static void AddFoundationServices(IServiceCollection services)
        {
            services.AddTransient<IDelegatedAccessService, DelegatedAccessService>();
            services.AddTransient<ILookupService, LookupService>();
            services.AddTransient<IOdsDataService, OdsDataService>();
            services.AddTransient<IPdsDataService, PdsDataService>();
            services.AddTransient<IUserAccessService, UserAccessService>();
        }

        private static void AddProcessingServices(IServiceCollection services)
        { }

        private static void AddOrchestrationServices(IServiceCollection services)
        { }

        private static void AddCoordinationServices(IServiceCollection services)
        { }
    }
}
