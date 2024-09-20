// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.OdsDatas;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tynamix.ObjectFiller;

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
                    d => d.ServiceType == typeof(IOdsStorageBroker));

                if (descriptor != null)
                    services.Remove(descriptor);

                Mock<OdsStorageBroker> mockOdsStorageBroker = SetupOdsStorageBrokerMock();

                services.AddTransient<IOdsStorageBroker>(_ => mockOdsStorageBroker.Object);
            });
        }

        private static Mock<OdsStorageBroker> SetupOdsStorageBrokerMock()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            var mockOdsStorageBroker = new Mock<OdsStorageBroker>(mockConfiguration.Object) { CallBase = true };

            //mockOdsStorageBroker.Setup(x =>
            //    x.SelectAsync<OdsData>(It.IsAny<Guid>()))
            //        .ReturnsAsync(CreateRandomOdsData());

            mockOdsStorageBroker.Setup(x =>
                x.SelectAllAsync<OdsData>())
                    .ReturnsAsync(CreateRandomOdsDatas()
                        .AsQueryable());

            return mockOdsStorageBroker;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static OdsData CreateRandomOdsData() =>
            CreateRandomOdsDataFiller().Create();

        private static Filler<OdsData> CreateRandomOdsDataFiller() =>
            new Filler<OdsData>();

        private static List<OdsData> CreateRandomOdsDatas()
        {
            int randomNumber = GetRandomNumber();
            List<OdsData> randomOdsDatas = new List<OdsData>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomOdsDatas.Add(CreateRandomOdsData());
            }

            return randomOdsDatas;
        }
    }
}