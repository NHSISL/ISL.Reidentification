using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Lookup UpdateLookupWithRandomValues(Lookup inputLookup)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<Lookup>();

            filler.Setup()
                .OnProperty(lookup => lookup.Id).Use(inputLookup.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(lookup => lookup.CreatedDate).Use(inputLookup.CreatedDate)
                .OnProperty(lookup => lookup.CreatedBy).Use(inputLookup.CreatedBy)
                .OnProperty(lookup => lookup.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<Lookup> PostRandomLookupAsync()
        {
            Lookup randomLookup = CreateRandomLookup();
            await this.apiBroker.PostLookupAsync(randomLookup);

            return randomLookup;
        }

        private async ValueTask<List<Lookup>> PostRandomLookupsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomLookups = new List<Lookup>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomLookups.Add(await PostRandomLookupAsync());
            }

            return randomLookups;
        }

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