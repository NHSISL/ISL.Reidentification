// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
    {
        [Fact]
        public async Task ShouldRemoveUserAccessByIdAsync()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            Guid inputUserAccessId = inputUserAccess.Id;
            UserAccess deletedUserAccess = inputUserAccess;
            UserAccess expectedUserAccess = deletedUserAccess.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.DeleteUserAccessAsync(inputUserAccess))
                    .ReturnsAsync(deletedUserAccess);

            // when
            UserAccess actualUserAccess =
                await this.userAccessService.RemoveUserAccessByIdAsync(inputUserAccessId);

            // then
            actualUserAccess.Should().BeEquivalentTo(expectedUserAccess);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.DeleteUserAccessAsync(inputUserAccess),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
