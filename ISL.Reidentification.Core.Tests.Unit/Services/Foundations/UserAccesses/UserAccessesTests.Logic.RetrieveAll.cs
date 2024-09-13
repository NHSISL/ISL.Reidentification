// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
    {
        [Fact]
        public async Task ShouldRetrieveAllUserAccessesAsync()
        {
            // given
            IQueryable<UserAccess> randomUserAccesses = CreateRandomUserAccesses();
            IQueryable<UserAccess> storageUserAccesses = randomUserAccesses;
            IQueryable<UserAccess> expectedUserAccesses = storageUserAccesses;

            this.ReIdentificationStorageBroker.Setup(broker =>
                broker.SelectAllUserAccessesAsync())
                    .ReturnsAsync(storageUserAccesses);

            // when
            IQueryable<UserAccess> actualUserAccesses = await this.userAccessService.RetrieveAllUserAccessesAsync();

            // then
            actualUserAccesses.Should().BeEquivalentTo(expectedUserAccesses);

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.SelectAllUserAccessesAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ReIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
