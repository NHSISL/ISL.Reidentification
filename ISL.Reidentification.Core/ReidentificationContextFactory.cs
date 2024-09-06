// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ISL.Reidentification.Core
{
    internal class ReidentificationContextFactory : IDesignTimeDbContextFactory<ReidentificationStorageBroker>
    {
        public ReidentificationStorageBroker CreateDbContext(string[] args)
        {
            List<KeyValuePair<string, string>> config = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(
                    key: "ConnectionStrings:ReidentificationConnection",
                    value: "Server=(localdb)\\MSSQLLocalDB;Database=Reidentification;" +
                        "Trusted_Connection=True;MultipleActiveResultSets=true"),
            };

            var configurationBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(initialData: config);

            IConfiguration configuration = configurationBuilder.Build();
            return new ReidentificationStorageBroker(configuration);
        }
    }
}
