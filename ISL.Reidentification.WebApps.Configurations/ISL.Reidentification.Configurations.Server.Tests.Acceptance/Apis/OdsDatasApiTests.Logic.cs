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
            // when
            var actualOdsDatas = await this.apiBroker.GetAllOdsDatasAsync();

            // then
            actualOdsDatas.Should().NotBeNull();
        }
    }
}
