// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions;
using ISL.ReIdentification.Core.Services.Foundations.ReIdentifications;
using Moq;
using Xeptions;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowAggregateExceptionForValidationErrorsOnBulkProcessIfRequestsFailsAsync(
            Xeption dependancyValidationException)
        {
            // Given
            Guid randomIdentifier = Guid.NewGuid();
            int batchSize = 1;
            int randomCount = 1;
            IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest(count: randomCount);
            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest;

            Mock<ReIdentificationService> reIdentificationServiceMock =
                new Mock<ReIdentificationService>(
                    this.necsBrokerMock.Object,
                    this.identifierBrokerMock.Object,
                    this.necsConfiguration,
                    this.loggingBrokerMock.Object)
                { CallBase = true };

            ReIdentificationService service = reIdentificationServiceMock.Object;

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ThrowsAsync(dependancyValidationException);

            var serverException =
                new Exception(message: GetRandomString());

            var failedClientReIdentificationException = new FailedClientReIdentificationException(
                message: "Failed NECS client error occurred, please contact support.",
                innerException: dependancyValidationException,
                data: dependancyValidationException.Data);

            var reidentificationDependencyValidationException = new ReIdentificationDependencyValidationException(
                message: "Re-identification dependency validation error occurred, fix errors and try again.",
                innerException: failedClientReIdentificationException);

            var expectedAggregateException = new AggregateException(
                message: $"Unable to process addresses in 1 of the batch(es) from {inputIdentificationRequest.Id}",
                innerExceptions: reidentificationDependencyValidationException);

            // When
            ValueTask<IdentificationRequest> reidentificationRequestTask = service
                .BulkProcessRequestsAsync(inputIdentificationRequest, batchSize);

            AggregateException actualAggregateException =
                await Assert.ThrowsAsync<AggregateException>(reidentificationRequestTask.AsTask);

            // Then
            actualAggregateException.Should().BeEquivalentTo(expectedAggregateException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   reidentificationDependencyValidationException))),
                       Times.Once);

            this.necsBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
