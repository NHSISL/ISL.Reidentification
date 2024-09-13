using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
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

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(randomLookup);

            LookupDependencyException actualLookupDependencyException =
                await Assert.ThrowsAsync<LookupDependencyException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupDependencyException.Should()
                .BeEquivalentTo(expectedLookupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(randomLookup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedLookupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(randomLookup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyIfReferenceErrorOccursAndLogItAsync()
        {
            // given
            Lookup someLookup = CreateRandomLookup();
            string randomMessage = GetRandomString();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidLookupReferenceException =
                new InvalidLookupReferenceException(
                    message: "Invalid lookup reference error occurred.", 
                    innerException: foreignKeyConstraintConflictException);

            LookupDependencyValidationException expectedLookupDependencyValidationException =
                new LookupDependencyValidationException(
                    message: "Lookup dependency validation occurred, please try again.",
                    innerException: invalidLookupReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(foreignKeyConstraintConflictException);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(someLookup);

            LookupDependencyValidationException actualLookupDependencyValidationException =
                await Assert.ThrowsAsync<LookupDependencyValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupDependencyValidationException.Should()
                .BeEquivalentTo(expectedLookupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(someLookup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedLookupDependencyValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(someLookup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            var databaseUpdateException = new DbUpdateException();

            var failedLookupStorageException =
                new FailedLookupStorageException(
                    message: "Failed lookup storage error occurred, contact support.",
                    innerException: databaseUpdateException);

            var expectedLookupDependencyException =
                new LookupDependencyException(
                    message: "Lookup dependency error occurred, contact support.",
                    innerException: failedLookupStorageException); 

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(randomLookup);

            LookupDependencyException actualLookupDependencyException =
                await Assert.ThrowsAsync<LookupDependencyException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupDependencyException.Should()
                .BeEquivalentTo(expectedLookupDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(randomLookup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(randomLookup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyErrorOccursAndLogAsync()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedLookupException =
                new LockedLookupException(
                    message: "Locked lookup record exception, please try again later",
                    innerException: databaseUpdateConcurrencyException);

            var expectedLookupDependencyValidationException =
                new LookupDependencyValidationException(
                    message: "Lookup dependency validation occurred, please try again.",
                    innerException: lockedLookupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(randomLookup);

            LookupDependencyValidationException actualLookupDependencyValidationException =
                await Assert.ThrowsAsync<LookupDependencyValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupDependencyValidationException.Should()
                .BeEquivalentTo(expectedLookupDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(randomLookup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(randomLookup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            var serviceException = new Exception();

            var failedLookupServiceException =
                new FailedLookupServiceException(
                    message: "Failed lookup service occurred, please contact support", 
                    innerException: serviceException);

            var expectedLookupServiceException =
                new LookupServiceException(
                    message: "Lookup service error occurred, contact support.",
                    innerException: failedLookupServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(randomLookup);

            LookupServiceException actualLookupServiceException =
                await Assert.ThrowsAsync<LookupServiceException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupServiceException.Should()
                .BeEquivalentTo(expectedLookupServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(randomLookup.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(randomLookup),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}