// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Ods
{
    public partial class OdsTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var failedStorageOdsDataException = new FailedStorageOdsDataException(
                message: "Failed ODS data storage error occurred, contact support.",
                innerException: sqlException);

            var expectedOdsDataDependencyException = new OdsDataDependencyException(
                message: "OdsData dependency error occurred, contact support.",
                innerException: failedStorageOdsDataException);

            this.odsStorageBroker.Setup(broker =>
                broker.SelectAllOdsDatasAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<OdsData>> retrieveAllOdsDatasTask =
                this.odsService.RetrieveAllOdsDatasAsync();

            OdsDataDependencyException actualOdsDataDependencyException =
                await Assert.ThrowsAsync<OdsDataDependencyException>(
                    retrieveAllOdsDatasTask.AsTask);

            // then
            actualOdsDataDependencyException.Should().BeEquivalentTo(expectedOdsDataDependencyException);

            this.odsStorageBroker.Verify(broker =>
                broker.SelectAllOdsDatasAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedOdsDataDependencyException))),
                        Times.Once());

            this.odsStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
