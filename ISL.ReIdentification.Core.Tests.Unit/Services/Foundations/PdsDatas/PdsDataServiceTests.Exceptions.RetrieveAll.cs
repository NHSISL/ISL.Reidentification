// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.PdsDatas
{
    public partial class PdsDataServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllWhenSQLExceptionOccursAndLogItAsync()
        {
            // given
            SqlException sqlException = CreateSqlException();

            var failedStoragePdsDataException =
                new FailedStoragePdsDataException(
                    message: "Failed pds data storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedPdsDataDependencyException =
                new PdsDataDependencyException(
                    message: "PdsData dependency error occurred, contact support.",
                    innerException: failedStoragePdsDataException);

            this.reIdentificationStorageBrokerMock.Setup(broker =>
                broker.SelectAllPdsDatasAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<IQueryable<PdsData>> retrieveAllPdsDataTask =
                this.pdsDataService.RetrieveAllPdsDatasAsync();

            PdsDataDependencyException actualPdsDataDependencyException =
                await Assert.ThrowsAsync<PdsDataDependencyException>(
                    testCode: retrieveAllPdsDataTask.AsTask);

            // then
            actualPdsDataDependencyException.Should().BeEquivalentTo(
                expectedPdsDataDependencyException);

            this.reIdentificationStorageBrokerMock.Verify(broker =>
                broker.SelectAllPdsDatasAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedPdsDataDependencyException))),
                        Times.Once);

            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceErrorOnRetrieveAllWhenServiceErrorOccursAndLogItAsync()
        {
            // given
            var serviceError = new Exception();

            var failedServicePdsDataException =
                new FailedServicePdsDataException(
                    message: "Failed pds data service error occurred, please contact support.",
                    innerException: serviceError);

            var expectedPdsDataServiceException =
                new PdsDataServiceException(
                    message: "PdsData service error occurred, contact support.",
                    innerException: failedServicePdsDataException);

            this.reIdentificationStorageBrokerMock.Setup(broker =>
                broker.SelectAllPdsDatasAsync())
                    .ThrowsAsync(serviceError);

            // when
            ValueTask<IQueryable<PdsData>> retrieveAllPdsDatasTask =
                this.pdsDataService.RetrieveAllPdsDatasAsync();

            PdsDataServiceException actualPdsDataServiceException =
                await Assert.ThrowsAsync<PdsDataServiceException>(
                    testCode: retrieveAllPdsDatasTask.AsTask);

            // then
            actualPdsDataServiceException.Should().BeEquivalentTo(
                expectedPdsDataServiceException);

            this.reIdentificationStorageBrokerMock.Verify(broker =>
                broker.SelectAllPdsDatasAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedPdsDataServiceException))),
                        Times.Once);

            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
