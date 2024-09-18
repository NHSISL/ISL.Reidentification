// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.Lookups;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis
{
    public partial class UserAccessesApiTests
    {
        [Fact]
        public async Task ShouldPostUserAccessAsync()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            UserAccess expectedUserAccess = inputUserAccess;

            // when
            await this.apiBroker.PostUserAccessAsync(inputUserAccess);

            UserAccess actualUserAccess =
                await this.apiBroker.GetUserAccessByIdAsync(inputUserAccess.Id);

            // then
            actualUserAccess.Should().BeEquivalentTo(expectedUserAccess);
            await this.apiBroker.DeleteUserAccessByIdAsync(inputUserAccess.Id);
        }
    }
}
