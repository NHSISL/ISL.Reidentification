using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
            ValueTask<Lookup> retrieveLookupByIdTask =
                this.lookupService.RetrieveLookupByIdAsync(someId);

            LookupDependencyException actualLookupDependencyException =
                await Assert.ThrowsAsync<LookupDependencyException>(
                    retrieveLookupByIdTask.AsTask);

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
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
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
            ValueTask<Lookup> retrieveLookupByIdTask =
                this.lookupService.RetrieveLookupByIdAsync(someId);

            LookupServiceException actualLookupServiceException =
                await Assert.ThrowsAsync<LookupServiceException>(
                    retrieveLookupByIdTask.AsTask);

            // then
            actualLookupServiceException.Should()
                .BeEquivalentTo(expectedLookupServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

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