// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using EFxceptions.Models.Exceptions;
using FluentAssertions;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccurredAndLogItAsync()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            SqlException sqlException = CreateSqlException();

            var failedStorageDelegatedAccessException =
                new FailedStorageDelegatedAccessException(
                    message: "Failed delegated access storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDelegatedAccessDependencyException =
                new DelegatedAccessDependencyException(
                    message: "DelegatedAccess dependency error occurred, contact support.",
                    innerException: failedStorageDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DelegatedAccess> addDelegatedAccessTask =
                this.delegatedAccessService.AddDelegatedAccessAsync(
                    someDelegatedAccess);

            DelegatedAccessDependencyException actualDelegatedAccessDependencyException =
                await Assert.ThrowsAsync<DelegatedAccessDependencyException>(
                    testCode: addDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessDependencyException.Should().BeEquivalentTo(
                expectedDelegatedAccessDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessDependencyException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfDelegatedAccessAlreadyExistsAndLogItAsync()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();

            var duplicateKeyException =
                new DuplicateKeyException(
                    message: "Duplicate key error occurred");

            var alreadyExistsDelegatedAccessException =
                new AlreadyExistsDelegatedAccessException(
                    message: "DelegatedAccess already exists error occurred.",
                    innerException: duplicateKeyException,
                    data: duplicateKeyException.Data);

            var expectedDelegatedAccessDependencyValidationException =
                new DelegatedAccessDependencyValidationException(
                    message: "DelegatedAccess dependency validation error occurred, fix errors and try again.",
                    innerException: alreadyExistsDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<DelegatedAccess> addDelegatedAccessTask =
                this.delegatedAccessService.AddDelegatedAccessAsync(someDelegatedAccess);

            DelegatedAccessDependencyValidationException actualDelegatedAccessDependencyValidationException =
                await Assert.ThrowsAsync<DelegatedAccessDependencyValidationException>(
                    testCode: addDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessDependencyValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessDependencyValidationException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccurredAndLogItAsync()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            var dbUpdateException = new DbUpdateException();

            var failedOperationDelegatedAccessException =
                new FailedOperationDelegatedAccessException(
                    message: "Failed operation delegated access error occurred, contact support.",
                    innerException: dbUpdateException);

            var expectedDelegatedAccessDependencyException =
                new DelegatedAccessDependencyException(
                    message: "DelegatedAccess dependency error occurred, contact support.",
                    innerException: failedOperationDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<DelegatedAccess> addDelegatedAccessTask =
                this.delegatedAccessService.AddDelegatedAccessAsync(
                    someDelegatedAccess);

            DelegatedAccessDependencyException actualDelegatedAccessDependencyException =
                await Assert.ThrowsAsync<DelegatedAccessDependencyException>(
                    testCode: addDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessDependencyException.Should().BeEquivalentTo(
                expectedDelegatedAccessDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessDependencyException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccurredAndLogItAsync()
        {
            //given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            var serviceException = new Exception();

            var failedServiceDelegatedAccessException =
                new FailedServiceDelegatedAccessException(
                    message: "Failed service delegated access error occurred, contact support.",
                    innerException: serviceException);

            var expectedDelegatedAccessServiceException =
                new DelegatedAccessServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<DelegatedAccess> addDelegatedAccessTask =
                this.delegatedAccessService.AddDelegatedAccessAsync(someDelegatedAccess);

            DelegatedAccessServiceException actualDelegatedAccessServiceException =
                await Assert.ThrowsAsync<DelegatedAccessServiceException>(
                    testCode: addDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessServiceException.Should().BeEquivalentTo(
                expectedDelegatedAccessServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessServiceException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }
    }
}
