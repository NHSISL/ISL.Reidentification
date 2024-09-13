using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            SqlException sqlException = GetSqlException();

            var failedLookupStorageException =
                new FailedLookupStorageException(
                    message: "Failed lookup storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedLookupDependencyException =
                new LookupDependencyException(
                    message: "Lookup dependency error occurred, contact support.",
                    innerException: failedLookupStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(randomLookup.Id))
                    .Throws(sqlException);

            // when
            ValueTask<Lookup> addLookupTask =
                this.lookupService.RemoveLookupByIdAsync(randomLookup.Id);

            LookupDependencyException actualLookupDependencyException =
                await Assert.ThrowsAsync<LookupDependencyException>(
                    addLookupTask.AsTask);

            // then
            actualLookupDependencyException.Should()
                .BeEquivalentTo(expectedLookupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(randomLookup.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLookupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLookupAsync(It.IsAny<Lookup>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someLookupId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedLookupException =
                new LockedLookupException(
                    message: "Locked lookup record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedLookupDependencyValidationException =
                new LookupDependencyValidationException(
                    message: "Lookup dependency validation occurred, please try again.",
                    innerException: lockedLookupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Lookup> removeLookupByIdTask =
                this.lookupService.RemoveLookupByIdAsync(someLookupId);

            LookupDependencyValidationException actualLookupDependencyValidationException =
                await Assert.ThrowsAsync<LookupDependencyValidationException>(
                    removeLookupByIdTask.AsTask);

            // then
            actualLookupDependencyValidationException.Should()
                .BeEquivalentTo(expectedLookupDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteLookupAsync(It.IsAny<Lookup>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someLookupId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedLookupStorageException =
                new FailedLookupStorageException(
                    message: "Failed lookup storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedLookupDependencyException =
                new LookupDependencyException(
                    message: "Lookup dependency error occurred, contact support.",
                    innerException: failedLookupStorageException); 

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Lookup> deleteLookupTask =
                this.lookupService.RemoveLookupByIdAsync(someLookupId);

            LookupDependencyException actualLookupDependencyException =
                await Assert.ThrowsAsync<LookupDependencyException>(
                    deleteLookupTask.AsTask);

            // then
            actualLookupDependencyException.Should()
                .BeEquivalentTo(expectedLookupDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLookupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someLookupId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedLookupServiceException =
                new FailedLookupServiceException(
                    message: "Failed lookup service occurred, please contact support", 
                    innerException: serviceException);

            var expectedLookupServiceException =
                new LookupServiceException(
                    message: "Lookup service error occurred, contact support.",
                    innerException: failedLookupServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Lookup> removeLookupByIdTask =
                this.lookupService.RemoveLookupByIdAsync(someLookupId);

            LookupServiceException actualLookupServiceException =
                await Assert.ThrowsAsync<LookupServiceException>(
                    removeLookupByIdTask.AsTask);

            // then
            actualLookupServiceException.Should()
                .BeEquivalentTo(expectedLookupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()),
                        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}