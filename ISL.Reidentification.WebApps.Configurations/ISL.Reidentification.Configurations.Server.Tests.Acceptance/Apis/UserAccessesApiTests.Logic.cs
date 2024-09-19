// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public async Task ShouldGetAllUserAccessesAsync()
        {
            // given
            List<UserAccess> randomUserAccesses = await PostRandomUserAccesses();
            List<UserAccess> expectedUserAccesses = randomUserAccesses;

            // when
            List<UserAccess> actualUserAccesses = await this.apiBroker.GetAllUserAccessesAsync();

            // then
            foreach (var expectedUserAccess in expectedUserAccesses)
            {
                UserAccess actualUserAccess = actualUserAccesses.Single(
                    actual => actual.Id == expectedUserAccess.Id);

                actualUserAccess.Should().BeEquivalentTo(expectedUserAccess);
                await this.apiBroker.DeleteUserAccessByIdAsync(actualUserAccess.Id);
            }
        }

        [Fact]
        public async Task ShouldGetUserAccessByIdAsync()
        {
            // given
            UserAccess randomUserAccess = await PostRandomUserAccess();
            UserAccess expectedUserAccess = randomUserAccess;

            // when
            UserAccess actualUserAccess = await this.apiBroker.GetUserAccessByIdAsync(randomUserAccess.Id);

            // then
            actualUserAccess.Should().BeEquivalentTo(expectedUserAccess);
            await this.apiBroker.DeleteUserAccessByIdAsync(actualUserAccess.Id);
        }
    }
}
