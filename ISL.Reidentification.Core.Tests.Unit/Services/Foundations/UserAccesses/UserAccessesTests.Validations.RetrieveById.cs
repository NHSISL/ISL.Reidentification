// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdWhenUserAccessIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidUserAccessId = Guid.Empty;
            var invalidUserAccessException = new InvalidUserAccessException(
                message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.Id),
                values: "Id is invalid");

            var expectedUserAccessValidationException =
                new UserAccessValidationException(
                    message: "UserAccess validation error occurred, please fix errors and try again.",
                    innerException: invalidUserAccessException);

            // when
            ValueTask<UserAccess> retrieveByIdUserAccessTask =
                this.userAccessService.RetrieveUserAccessByIdAsync(invalidUserAccessId);

            UserAccessValidationException actualUserAccessValidationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(retrieveByIdUserAccessTask.AsTask);

            // then
            actualUserAccessValidationException.Should().BeEquivalentTo(expectedUserAccessValidationException);
            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedUserAccessValidationException))), Times.Once());

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.SelectUserAccessByIdAsync(invalidUserAccessId),
                    Times.Never);

            this.ReIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
