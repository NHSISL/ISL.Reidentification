// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            SqlException sqlException = CreateSqlException();

            var failedDelegatedAccessStorageException =
                new FailedStorageDelegatedAccessException(
                    message: "Failed delegated access storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedDelegatedAccessDependencyException =
                new DelegatedAccessDependencyException(
                    message: "DelegatedAccess dependency error occurred, contact support.",
                        innerException: failedDelegatedAccessStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(someDelegatedAccess);

            DelegatedAccessDependencyException actualDelegatedAccessDependencyException =
                await Assert.ThrowsAsync<DelegatedAccessDependencyException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessDependencyException.Should().BeEquivalentTo(
                expectedDelegatedAccessDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccess.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessDependencyException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(someDelegatedAccess),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DelegatedAccess randomDelegatedAccess =
                CreateRandomDelegatedAccess(randomDateTimeOffset);

            randomDelegatedAccess.CreatedDate =
                randomDateTimeOffset.AddMinutes(minutesInPast);

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
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(randomDelegatedAccess);

            DelegatedAccessDependencyException actualDelegatedAccessDependencyException =
                await Assert.ThrowsAsync<DelegatedAccessDependencyException>(
                    modifyDelegatedAccessTask.AsTask);

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
                broker.SelectDelegatedAccessByIdAsync(randomDelegatedAccess.Id),
                    Times.Never);

            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess(randomDateTimeOffset);

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedDelegatedAccessException =
                new LockedDelegatedAccessException(
                    message: "Locked delegated access record error occurred, please try again.",
                    innerException: dbUpdateConcurrencyException);

            var expectedDelegatedAccessDependencyValidationException =
                new DelegatedAccessDependencyValidationException(
                    message: "DelegatedAccess dependency validation error occurred, fix errors and try again.",
                    innerException: lockedDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(randomDelegatedAccess);

            DelegatedAccessDependencyValidationException actualDelegatedAccessDependencyValidationException =
                await Assert.ThrowsAsync<DelegatedAccessDependencyValidationException>(
                    modifyDelegatedAccessTask.AsTask);

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
                broker.SelectDelegatedAccessByIdAsync(randomDelegatedAccess.Id),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            DelegatedAccess randomDelegatedAccess =
                CreateRandomDelegatedAccess(randomDateTimeOffset);

            randomDelegatedAccess.CreatedDate =
                randomDateTimeOffset.AddMinutes(minutesInPast);

            var serviceException = new Exception();

            var failedServiceDelegatedAccessException =
                new FailedServiceDelegatedAccessException(
                    message: "Failed service delegated access error occurred, contact support.",
                    innerException: serviceException);

            var expectedDelegatedAccessServiceException =
                new DelegatedAccessServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceDelegatedAccessException);

            this.reidentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(randomDelegatedAccess.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(randomDelegatedAccess);

            DelegatedAccessServiceException actualDelegatedAccessServiceException =
                await Assert.ThrowsAsync<DelegatedAccessServiceException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessServiceException.Should().BeEquivalentTo(
                expectedDelegatedAccessServiceException);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(randomDelegatedAccess.Id),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessServiceException))),
                        Times.Once);

            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
