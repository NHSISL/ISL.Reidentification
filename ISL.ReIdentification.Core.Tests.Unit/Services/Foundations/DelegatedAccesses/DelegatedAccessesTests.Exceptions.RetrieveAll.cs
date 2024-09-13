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
    }
}
