// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis.PdsDatas
{
    public partial class PdsDataApiTests
    {
        [Fact]
        public async Task ShouldGetAllPdsDataAsync()
        {
            // when
            var actualPdsDatas = await this.apiBroker.GetAllPdsDataAsync();

            // then
            actualPdsDatas.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldGetPdsDataByIdAsync()
        {
            // given
            Guid randomPdsDataId = Guid.NewGuid();

            // when
            var actualPdsData = await this.apiBroker.GetOdsDataByIdAsync(randomPdsDataId);

            // then
            actualPdsData.Should().NotBeNull();
        }
    }
}
