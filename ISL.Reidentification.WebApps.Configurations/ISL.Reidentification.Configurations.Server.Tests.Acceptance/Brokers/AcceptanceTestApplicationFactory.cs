// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers
{
    public class AcceptanceTestApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder); // Call base implementation to inherit configuration

            builder.ConfigureServices(services =>
            {
                // Remove or override services if needed
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IPatientOrgReferenceStorageBroker));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddTransient<IPatientOrgReferenceStorageBroker, MockPatientOrgReferenceStorageBroker>();
            });
        }
    }
}