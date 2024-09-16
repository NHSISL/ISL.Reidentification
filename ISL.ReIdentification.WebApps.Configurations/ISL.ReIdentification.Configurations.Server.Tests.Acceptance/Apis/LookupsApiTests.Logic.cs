using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.Lookups;
using Xunit;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis.Lookups
{
    public partial class LookupsApiTests
    {
        [Fact]
        public async Task ShouldPostLookupAsync()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            Lookup expectedLookup = inputLookup;

            // when 
            await this.apiBroker.PostLookupAsync(inputLookup);

            Lookup actualLookup =
                await this.apiBroker.GetLookupByIdAsync(inputLookup.Id);

            // then
            actualLookup.Should().BeEquivalentTo(expectedLookup);
            await this.apiBroker.DeleteLookupByIdAsync(actualLookup.Id);
        }

        [Fact]
        public async Task ShouldGetAllLookupsAsync()
        {
            // given
            List<Lookup> randomLookups = await PostRandomLookupsAsync();
            List<Lookup> expectedLookups = randomLookups;

            // when
            List<Lookup> actualLookups = await this.apiBroker.GetAllLookupsAsync();

            // then
            foreach (Lookup expectedLookup in expectedLookups)
            {
                Lookup actualLookup = actualLookups.Single(approval => approval.Id == expectedLookup.Id);
                actualLookup.Should().BeEquivalentTo(expectedLookup);
                await this.apiBroker.DeleteLookupByIdAsync(actualLookup.Id);
            }
        }

        [Fact]
        public async Task ShouldGetLookupAsync()
        {
            // given
            Lookup randomLookup = await PostRandomLookupAsync();
            Lookup expectedLookup = randomLookup;

            // when
            Lookup actualLookup = await this.apiBroker.GetLookupByIdAsync(randomLookup.Id);

            // then
            actualLookup.Should().BeEquivalentTo(expectedLookup);
            await this.apiBroker.DeleteLookupByIdAsync(actualLookup.Id);
        }
    }
}