// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSQLExceptionOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var failedStorageDelegatedAccessException =
                new FailedStorageDelegatedAccessException(
                    message: "Failed delegated access storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDelegatedAccessDependencyException =
                new DelegatedAccessDependencyException(
                    message: "DelegatedAccess dependency error occurred, contact support.",
                    innerException: failedStorageDelegatedAccessException);

            this.ReIdentificationStorageBroker.Setup(broker =>
                broker.SelectAllDelegatedAccessesAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<DelegatedAccess>> retrieveAllDelegatedAccessesTask =
                this.delegatedAccessService.RetrieveAllDelegatedAccessesAsync();

            DelegatedAccessDependencyException actualDelegatedAccessDependencyException =
                await Assert.ThrowsAsync<DelegatedAccessDependencyException>(
                    testCode: retrieveAllDelegatedAccessesTask.AsTask);

            // then
            actualDelegatedAccessDependencyException.Should().BeEquivalentTo(
                expectedDelegatedAccessDependencyException);

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.SelectAllDelegatedAccessesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessDependencyException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.ReIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceErrorOnRetrieveAllWhenServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceError = new Exception();

            var failedServiceDelegatedAccessException =
                new FailedServiceDelegatedAccessException(
                    message: "Failed service delegated access error occurred, contact support.",
                    innerException: serviceError);

            var expectedDelegatedAccessServiceException =
                new DelegatedAccessServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceDelegatedAccessException);

            this.ReIdentificationStorageBroker.Setup(broker =>
                broker.SelectAllDelegatedAccessesAsync())
                    .ThrowsAsync(serviceError);

            // when
            ValueTask<IQueryable<DelegatedAccess>> retrieveAllDelegatedAccesssTask =
                this.delegatedAccessService.RetrieveAllDelegatedAccessesAsync();

            DelegatedAccessServiceException actualDelegatedAccessServiceException =
                await Assert.ThrowsAsync<DelegatedAccessServiceException>(
                    testCode: retrieveAllDelegatedAccesssTask.AsTask);

            // then
            actualDelegatedAccessServiceException.Should().BeEquivalentTo(
                expectedDelegatedAccessServiceException);

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.SelectAllDelegatedAccessesAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessServiceException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.ReIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
