// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ISL.ReIdentification.Core
{
    internal class PatientOrgReferenceContextFactory : IDesignTimeDbContextFactory<PatientOrgReferenceStorageBroker>
    {
        public PatientOrgReferenceStorageBroker CreateDbContext(string[] args)
        {
            List<KeyValuePair<string, string>> config = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    key: "ConnectionStrings:OdsConnection",
                    value: "Server=(localdb)\\MSSQLLocalDB;Database=OdsLookups;" +
                        "Trusted_Connection=True;MultipleActiveResultSets=true"),
            };

            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData: config);

            IConfiguration configuration = configurationBuilder.Build();
            return new PatientOrgReferenceStorageBroker(configuration);
        }
    }
}
