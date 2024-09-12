// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            SqlException sqlException = CreateSqlException();

            var failedUserAccessStorageException =
                new FailedStorageUserAccessException(
                    message: "Failed user access storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedUserAccessDependencyException =
                new UserAccessDependencyException(
                    message: "UserAccess dependency error occurred, contact support.",
                        innerException: failedUserAccessStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(randomUserAccess);

            UserAccessDependencyException actualUserAccessDependencyException =
                await Assert.ThrowsAsync<UserAccessDependencyException>(
                    modifyUserAccessTask.AsTask);

            // then
            actualUserAccessDependencyException.Should().BeEquivalentTo(
                expectedUserAccessDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.SelectUserAccessByIdAsync(randomUserAccess.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedUserAccessDependencyException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.UpdateUserAccessAsync(randomUserAccess),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDependencyErrorOccurredAndLogItAsync()
        {
            // given
            UserAccess someUserAccess = CreateRandomUserAccess();
            var dbUpdateException = new DbUpdateException();

            var failedOperationUserAccessException =
                new FailedOperationUserAccessException(
                    message: "Failed operation user access error occurred, contact support.",
                    innerException: dbUpdateException);

            var expectedUserAccessDependencyException =
                new UserAccessDependencyException(
                    message: "UserAccess dependency error occurred, contact support.",
                    innerException: failedOperationUserAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<UserAccess> addUserAccessTask =
                this.userAccessService.AddUserAccessAsync(
                    someUserAccess);

            UserAccessDependencyException actualUserAccessDependencyException =
                await Assert.ThrowsAsync<UserAccessDependencyException>(
                    testCode: addUserAccessTask.AsTask);

            // then
            actualUserAccessDependencyException.Should().BeEquivalentTo(
                expectedUserAccessDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedUserAccessDependencyException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(It.IsAny<UserAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }
    }
}
