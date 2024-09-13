// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
    {
        [Fact]
        public async Task ShouldModifyUserAccessAsync()
        {
            // given
            DateTimeOffset randomDateOffset = GetRandomDateTimeOffset();

            UserAccess randomModifyUserAccess =
                CreateRandomModifyUserAccess(randomDateOffset);

            UserAccess inputUserAccess = randomModifyUserAccess.DeepClone();
            UserAccess storageUserAccess = randomModifyUserAccess.DeepClone();
            storageUserAccess.UpdatedDate = storageUserAccess.CreatedDate;
            UserAccess updatedUserAccess = inputUserAccess.DeepClone();
            UserAccess expectedUserAccess = updatedUserAccess.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateOffset);

            this.ReIdentificationStorageBroker.Setup(broker =>
                broker.SelectUserAccessByIdAsync(inputUserAccess.Id))
                    .ReturnsAsync(storageUserAccess);

            this.ReIdentificationStorageBroker.Setup(broker =>
                broker.UpdateUserAccessAsync(inputUserAccess))
                    .ReturnsAsync(updatedUserAccess);

            // when
            UserAccess actualUserAccess =
                await this.userAccessService.ModifyUserAccessAsync(inputUserAccess);

            // then
            actualUserAccess.Should().BeEquivalentTo(expectedUserAccess);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.SelectUserAccessByIdAsync(inputUserAccess.Id),
                    Times.Once);

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.UpdateUserAccessAsync(inputUserAccess),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.ReIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
