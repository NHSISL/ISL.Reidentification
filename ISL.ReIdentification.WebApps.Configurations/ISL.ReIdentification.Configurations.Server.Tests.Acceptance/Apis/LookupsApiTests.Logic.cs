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
    }
}