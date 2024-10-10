﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ImpersonationContexts
{
    public partial class ImpersonationContextsTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            ImpersonationContext someImpersonationContext = CreateRandomImpersonationContext();
            SqlException sqlException = CreateSqlException();

            var failedImpersonationContextStorageException =
                new FailedStorageImpersonationContextException(
                    message: "Failed impersonation context storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedImpersonationContextDependencyException =
                new ImpersonationContextDependencyException(
                    message: "ImpersonationContext dependency error occurred, contact support.",
                        innerException: failedImpersonationContextStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<ImpersonationContext> modifyImpersonationContextTask =
                this.impersonationContextService.ModifyImpersonationContextAsync(someImpersonationContext);

            ImpersonationContextDependencyException actualImpersonationContextDependencyException =
                await Assert.ThrowsAsync<ImpersonationContextDependencyException>(
                    modifyImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextDependencyException.Should().BeEquivalentTo(
                expectedImpersonationContextDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectImpersonationContextByIdAsync(someImpersonationContext.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedImpersonationContextDependencyException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.UpdateImpersonationContextAsync(someImpersonationContext),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ImpersonationContext randomImpersonationContext =
                CreateRandomImpersonationContext(randomDateTimeOffset);

            randomImpersonationContext.CreatedDate =
                randomDateTimeOffset.AddMinutes(minutesInPast);

            var dbUpdateException = new DbUpdateException();

            var failedOperationImpersonationContextException =
                new FailedOperationImpersonationContextException(
                    message: "Failed operation impersonation context error occurred, contact support.",
                    innerException: dbUpdateException);

            var expectedImpersonationContextDependencyException =
                new ImpersonationContextDependencyException(
                    message: "ImpersonationContext dependency error occurred, contact support.",
                    innerException: failedOperationImpersonationContextException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dbUpdateException);

            // when
            ValueTask<ImpersonationContext> modifyImpersonationContextTask =
                this.impersonationContextService.ModifyImpersonationContextAsync(randomImpersonationContext);

            ImpersonationContextDependencyException actualImpersonationContextDependencyException =
                await Assert.ThrowsAsync<ImpersonationContextDependencyException>(
                    modifyImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextDependencyException.Should().BeEquivalentTo(
                expectedImpersonationContextDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedImpersonationContextDependencyException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectImpersonationContextByIdAsync(randomImpersonationContext.Id),
                    Times.Never);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyValidationExceptionOnModifyIfDbUpdateConcurrencyOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext(randomDateTimeOffset);

            var dbUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedImpersonationContextException =
                new LockedImpersonationContextException(
                    message: "Locked impersonation context record error occurred, please try again.",
                    innerException: dbUpdateConcurrencyException);

            var expectedImpersonationContextDependencyValidationException =
                new ImpersonationContextDependencyValidationException(
                    message: "ImpersonationContext dependency validation error occurred, fix errors and try again.",
                    innerException: lockedImpersonationContextException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dbUpdateConcurrencyException);

            // when
            ValueTask<ImpersonationContext> modifyImpersonationContextTask =
                this.impersonationContextService.ModifyImpersonationContextAsync(randomImpersonationContext);

            ImpersonationContextDependencyValidationException actualImpersonationContextDependencyValidationException =
                await Assert.ThrowsAsync<ImpersonationContextDependencyValidationException>(
                    modifyImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextDependencyValidationException.Should().BeEquivalentTo(
                expectedImpersonationContextDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedImpersonationContextDependencyValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectImpersonationContextByIdAsync(randomImpersonationContext.Id),
                    Times.Never());

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceErrorOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();

            ImpersonationContext randomImpersonationContext =
                CreateRandomImpersonationContext(randomDateTimeOffset);

            randomImpersonationContext.CreatedDate =
                randomDateTimeOffset.AddMinutes(minutesInPast);

            var serviceException = new Exception();

            var failedServiceImpersonationContextException =
                new FailedServiceImpersonationContextException(
                    message: "Failed service impersonation context error occurred, contact support.",
                    innerException: serviceException);

            var expectedImpersonationContextServiceException =
                new ImpersonationContextServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceImpersonationContextException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectImpersonationContextByIdAsync(randomImpersonationContext.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ImpersonationContext> modifyImpersonationContextTask =
                this.impersonationContextService.ModifyImpersonationContextAsync(randomImpersonationContext);

            ImpersonationContextServiceException actualImpersonationContextServiceException =
                await Assert.ThrowsAsync<ImpersonationContextServiceException>(
                    modifyImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextServiceException.Should().BeEquivalentTo(
                expectedImpersonationContextServiceException);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectImpersonationContextByIdAsync(randomImpersonationContext.Id),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedImpersonationContextServiceException))),
                        Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}