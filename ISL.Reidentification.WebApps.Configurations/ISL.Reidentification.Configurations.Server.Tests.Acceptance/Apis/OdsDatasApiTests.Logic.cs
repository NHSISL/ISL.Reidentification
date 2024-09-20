// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis
{
    public partial class OdsDatasApiTests
    {
        [Fact]
        public async Task ShouldGetAllOdsDatasAsync()
        {
            // given
            //List<OdsData> randomOdsDatas = CreateRandomOdsDatas();
            //List<OdsData> storageOdsDatas = randomOdsDatas;
            //List<OdsData> expectedOdsDatas = storageOdsDatas;
            // when

            var actualOdsDatas = await this.apiBroker.GetAllOdsDatasAsync();

            // then
            actualOdsDatas.Should().NotBeNull();

        }
    }
}
