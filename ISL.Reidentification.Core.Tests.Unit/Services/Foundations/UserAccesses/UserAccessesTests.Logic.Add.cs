// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
    {
        [Fact]
        public async Task ShouldModifyUserAccessAsync()
        {
            // given
            //given
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

            this.reidentificationStorageBroker.Setup(broker =>
                broker.SelectUserAccessByIdAsync(inputUserAccess.Id))
                    .ReturnsAsync(storageUserAccess);

            this.reidentificationStorageBroker.Setup(broker =>
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

            this.reidentificationStorageBroker.Verify(broker =>
                broker.UpdateUserAccessAsync(inputUserAccess),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
