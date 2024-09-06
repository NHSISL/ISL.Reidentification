// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;

namespace ISL.Reidentification.Configurations.Server.Tests.Acceptance.Apis.Home
{
    public partial class FeaturesApiTests
    {

        [Fact]
        public async Task ShouldGetHomeAsync()
        {
            // Given
            string expectedResult = "Hello, Mario. The princess is in another castle.";

            // When
            string actualResult = await this.apiBroker.GetFeaturesAsync();

            // Then
            actualResult.Should().BeEquivalentTo(expectedResult);

        }
    }
}