﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.CsvIdentificationRequests
{
    public partial class CsvIdentificationRequestsTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccurredAndLogItAsync()
        {
            // given
            CsvIdentificationRequest someCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();
            SqlException sqlException = CreateSqlException();

            var failedStorageCsvIdentificationRequestException =
                new FailedStorageCsvIdentificationRequestException(
                    message: "Failed delegated access storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedCsvIdentificationRequestDependencyException =
                new CsvIdentificationRequestDependencyException(
                    message: "CsvIdentificationRequest dependency error occurred, contact support.",
                    innerException: failedStorageCsvIdentificationRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CsvIdentificationRequest> addCsvIdentificationRequestTask =
                this.csvIdentificationRequestService.AddCsvIdentificationRequestAsync(
                    someCsvIdentificationRequest);

            CsvIdentificationRequestDependencyException actualCsvIdentificationRequestDependencyException =
                await Assert.ThrowsAsync<CsvIdentificationRequestDependencyException>(
                    testCode: addCsvIdentificationRequestTask.AsTask);

            // then
            actualCsvIdentificationRequestDependencyException.Should().BeEquivalentTo(
                expectedCsvIdentificationRequestDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedCsvIdentificationRequestDependencyException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfCsvIdentificationRequestAlreadyExistsAndLogItAsync()
        {
            // given
            CsvIdentificationRequest someCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();

            var duplicateKeyException =
                new DuplicateKeyException(
                    message: "Duplicate key error occurred");

            var alreadyExistsCsvIdentificationRequestException =
                new AlreadyExistsCsvIdentificationRequestException(
                    message: "CsvIdentificationRequest already exists error occurred.",
                    innerException: duplicateKeyException,
                    data: duplicateKeyException.Data);

            var expectedCsvIdentificationRequestDependencyValidationException =
                new CsvIdentificationRequestDependencyValidationException(
                    message: "CsvIdentificationRequest dependency validation error occurred, fix errors and try again.",
                    innerException: alreadyExistsCsvIdentificationRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<CsvIdentificationRequest> addCsvIdentificationRequestTask =
                this.csvIdentificationRequestService.AddCsvIdentificationRequestAsync(someCsvIdentificationRequest);

            CsvIdentificationRequestDependencyValidationException actualCsvIdentificationRequestDependencyValidationException =
                await Assert.ThrowsAsync<CsvIdentificationRequestDependencyValidationException>(
                    testCode: addCsvIdentificationRequestTask.AsTask);

            // then
            actualCsvIdentificationRequestDependencyValidationException.Should().BeEquivalentTo(
                expectedCsvIdentificationRequestDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedCsvIdentificationRequestDependencyValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDependencyErrorOccurredAndLogItAsync()
        {
            // given
            CsvIdentificationRequest someCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();
            var dbUpdateException = new DbUpdateException();

            var failedOperationCsvIdentificationRequestException =
                new FailedOperationCsvIdentificationRequestException(
                    message: "Failed operation delegated access error occurred, contact support.",
                    innerException: dbUpdateException);

            var expectedCsvIdentificationRequestDependencyException =
                new CsvIdentificationRequestDependencyException(
                    message: "CsvIdentificationRequest dependency error occurred, contact support.",
                    innerException: failedOperationCsvIdentificationRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<CsvIdentificationRequest> addCsvIdentificationRequestTask =
                this.csvIdentificationRequestService.AddCsvIdentificationRequestAsync(
                    someCsvIdentificationRequest);

            CsvIdentificationRequestDependencyException actualCsvIdentificationRequestDependencyException =
                await Assert.ThrowsAsync<CsvIdentificationRequestDependencyException>(
                    testCode: addCsvIdentificationRequestTask.AsTask);

            // then
            actualCsvIdentificationRequestDependencyException.Should().BeEquivalentTo(
                expectedCsvIdentificationRequestDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedCsvIdentificationRequestDependencyException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccurredAndLogItAsync()
        {
            //given
            CsvIdentificationRequest someCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();
            var serviceException = new Exception();

            var failedServiceCsvIdentificationRequestException =
                new FailedServiceCsvIdentificationRequestException(
                    message: "Failed service delegated access error occurred, contact support.",
                    innerException: serviceException);

            var expectedCsvIdentificationRequestServiceException =
                new CsvIdentificationRequestServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceCsvIdentificationRequestException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<CsvIdentificationRequest> addCsvIdentificationRequestTask =
                this.csvIdentificationRequestService.AddCsvIdentificationRequestAsync(someCsvIdentificationRequest);

            CsvIdentificationRequestServiceException actualCsvIdentificationRequestServiceException =
                await Assert.ThrowsAsync<CsvIdentificationRequestServiceException>(
                    testCode: addCsvIdentificationRequestTask.AsTask);

            // then
            actualCsvIdentificationRequestServiceException.Should().BeEquivalentTo(
                expectedCsvIdentificationRequestServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedCsvIdentificationRequestServiceException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }
    }
}