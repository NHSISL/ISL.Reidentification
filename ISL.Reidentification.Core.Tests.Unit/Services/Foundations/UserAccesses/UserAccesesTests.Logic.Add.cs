// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccesesTests
    {
        [Fact]
        public async Task ShouldAddUserAccessAsync()
        {
            //given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            UserAccess storageUserAccess = inputUserAccess.DeepClone();
            UserAccess expectedUserAccess = inputUserAccess.DeepClone();

            this.reidentificationStorageBroker.Setup(broker =>
                broker.InsertUserAccessAsync(inputUserAccess))
                    .ReturnsAsync(storageUserAccess);

            //when
            UserAccess actualUserAccess = await this.userAccessService.AddUserAccessAsync(inputUserAccess);

            //then
            actualUserAccess.Should().BeEquivalentTo(expectedUserAccess);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(inputUserAccess),
                    Times.Once);

            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
