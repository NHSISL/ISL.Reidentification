// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.DelegatedAccesses;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis
{
    public partial class DelegatedAccessesApiTests
    {
        [Fact]
        public async Task ShouldPostDelegatedAccessAsync()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess expectedDelegatedAccess = inputDelegatedAccess;

            // when 
            await this.apiBroker.PostDelegatedAccessAsync(inputDelegatedAccess);

            DelegatedAccess actualDelegatedAccess =
                await this.apiBroker.GetDelegatedAccessByIdAsync(inputDelegatedAccess.Id);

            // then
            actualDelegatedAccess.Should().BeEquivalentTo(expectedDelegatedAccess);
            await this.apiBroker.DeleteDelegatedAccessByIdAsync(actualDelegatedAccess.Id);
        }

        [Fact]
        public async Task ShouldGetDelegatedAccessByIdAsync()
        {
            // given
            DelegatedAccess randomDelegatedAccess = await PostRandomDelegatedAccessAsync();
            DelegatedAccess expectedDelegatedAccess = randomDelegatedAccess;

            // when
            var actualDelegatedAccess = await this.apiBroker.GetDelegatedAccessByIdAsync(randomDelegatedAccess.Id);

            // then
            actualDelegatedAccess.Should().BeEquivalentTo(expectedDelegatedAccess);
            await this.apiBroker.DeleteDelegatedAccessByIdAsync(actualDelegatedAccess.Id);
        }
    }
}
