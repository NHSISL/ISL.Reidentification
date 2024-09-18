// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccurredAndLogItAsync()
        {
            // given
            IdentificationRequest someIdentificationRequest = CreateRandomIdentificationRequest();
            var serviceException = new Exception();

            var failedServiceReIdentificationException =
                new FailedServiceReIdentificationException(
                    message: "Failed service re identification error occurred, contact support.",
                    innerException: serviceException);

            var expectedReIdentificationServiceException =
                new ReIdentificationServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: failedServiceReIdentificationException);

            this.necsBrokerMock.Setup(broker =>
                broker.ReIdAsync(It.IsAny<string>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<IdentificationRequest> addIdentificationRequestTask =
                this.reIdentificationService.ProcessReidentificationRequest(someIdentificationRequest);

            ReIdentificationServiceException actualReIdentificationServiceException =
                await Assert.ThrowsAsync<ReIdentificationServiceException>(
                    testCode: addIdentificationRequestTask.AsTask);

            // then
            actualReIdentificationServiceException.Should().BeEquivalentTo(
                expectedReIdentificationServiceException);

            this.necsBrokerMock.Verify(broker =>
                broker.ReIdAsync(It.IsAny<string>()), 
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedReIdentificationServiceException))),
                        Times.Once);

            this.necsBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
