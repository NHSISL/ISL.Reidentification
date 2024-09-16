// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRemoveByIdIfSQLErrorOccursAndLogItAsync()
        {
            // given
            var someDelegatedAccessId = Guid.NewGuid();
            var sqlException = CreateSqlException();

            var failedStorageDelegatedAccessException =
                new FailedStorageDelegatedAccessException(
                    message: "Failed delegated access storage error occurred, contact support.",
                    innerException: sqlException);

            var expectedDelegatedAccessDependencyException =
                new DelegatedAccessDependencyException(
                    message: "DelegatedAccess dependency error occurred, contact support.",
                    innerException: failedStorageDelegatedAccessException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccessId))
                        .ThrowsAsync(sqlException);

            // when
            ValueTask<DelegatedAccess> removeDelegatedAccessByIdTask =
                this.delegatedAccessService.RemoveDelegatedAccessByIdAsync(someDelegatedAccessId);

            // then
            await Assert.ThrowsAsync<DelegatedAccessDependencyException>(() =>
                removeDelegatedAccessByIdTask.AsTask());

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccessId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCriticalAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Never);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            var someDelegatedAccessId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedServiceDelegatedAccessException =
                new FailedServiceDelegatedAccessException(
                    message: "Failed service delegated access error occurred, contact support.",
                    innerException: serviceException);

            var expectedDelegatedAccessServiceException =
                new DelegatedAccessServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceDelegatedAccessException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<DelegatedAccess> removeDelegatedAccessByIdTask =
                this.delegatedAccessService.RemoveDelegatedAccessByIdAsync(someDelegatedAccessId);

            DelegatedAccessServiceException actualDelegatedAccessServiceException =
                await Assert.ThrowsAsync<DelegatedAccessServiceException>(
                    removeDelegatedAccessByIdTask.AsTask);

            // then
            actualDelegatedAccessServiceException.Should().BeEquivalentTo(expectedDelegatedAccessServiceException);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccessId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessServiceException))),
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
