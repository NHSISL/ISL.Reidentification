using System;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.Lookups;
using Tynamix.ObjectFiller;
using Xunit;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis.Lookups
{
    [Collection(nameof(ApiTestCollection))]
    public partial class LookupsApiTests
    {
        private readonly ApiBroker apiBroker;

        public LookupsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static Lookup CreateRandomLookup() =>
            CreateRandomLookupFiller().Create();

        private static Filler<Lookup> CreateRandomLookupFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<Lookup>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(lookup => lookup.CreatedDate).Use(now)
                .OnProperty(lookup => lookup.CreatedBy).Use(user)
                .OnProperty(lookup => lookup.UpdatedDate).Use(now)
                .OnProperty(lookup => lookup.UpdatedBy).Use(user);

            return filler;
        }
    }
}