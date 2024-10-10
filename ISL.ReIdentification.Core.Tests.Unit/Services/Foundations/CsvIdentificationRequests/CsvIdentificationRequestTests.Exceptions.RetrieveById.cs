﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.CsvIdentificationRequests
{
    public partial class CsvIdentificationRequestsTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSQLErrorOccursAndLogItAsync()
        {
            // given
            var someCsvIdentificationRequestId = Guid.NewGuid();
            var sqlException = CreateSqlException();

            var failedStorageCsvIdentificationRequestException =
                new FailedStorageCsvIdentificationRequestException(
                    message: "Failed delegated access storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedCsvIdentificationRequestDependencyException =
                new CsvIdentificationRequestDependencyException(
                    message: "CsvIdentificationRequest dependency error occurred, contact support.",
                    innerException: failedStorageCsvIdentificationRequestException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectCsvIdentificationRequestByIdAsync(someCsvIdentificationRequestId))
                        .ThrowsAsync(sqlException);

            // when
            ValueTask<CsvIdentificationRequest> retrieveCsvIdentificationRequestByIdTask =
                this.csvIdentificationRequestService.RetrieveCsvIdentificationRequestByIdAsync(someCsvIdentificationRequestId);

            // then
            await Assert.ThrowsAsync<CsvIdentificationRequestDependencyException>(() =>
                retrieveCsvIdentificationRequestByIdTask.AsTask());

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectCsvIdentificationRequestByIdAsync(someCsvIdentificationRequestId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedCsvIdentificationRequestDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            var someCsvIdentificationRequestId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedServiceCsvIdentificationRequestException =
                new FailedServiceCsvIdentificationRequestException(
                    message: "Failed service delegated access error occurred, contact support.",
                    innerException: serviceException);

            var expectedCsvIdentificationRequestServiceException =
                new CsvIdentificationRequestServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceCsvIdentificationRequestException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectCsvIdentificationRequestByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<CsvIdentificationRequest> retrieveCsvIdentificationRequestByIdTask =
                this.csvIdentificationRequestService.RetrieveCsvIdentificationRequestByIdAsync(someCsvIdentificationRequestId);

            CsvIdentificationRequestServiceException actualCsvIdentificationRequestServiceException =
                await Assert.ThrowsAsync<CsvIdentificationRequestServiceException>(
                    retrieveCsvIdentificationRequestByIdTask.AsTask);

            //then
            actualCsvIdentificationRequestServiceException.Should().BeEquivalentTo(
                expectedCsvIdentificationRequestServiceException);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectCsvIdentificationRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogErrorAsync(It.Is(SameExceptionAs(
                expectedCsvIdentificationRequestServiceException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}