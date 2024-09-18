// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnProcessIdentificationRequestAsync()
        {
            // given
            IdentificationRequest nullIdentificationRequest = null;

            var nullIdentificationRequestException =
                new NullIdentificationRequestException(message: "Identification request is null.");

            var expectedIdentificationRequestValidationException =
                new IdentificationRequestValidationException(
                    message: "Re identification validation error occurred, please fix errors and try again.",
                    innerException: nullIdentificationRequestException);

            // when
            ValueTask<IdentificationRequest> processdentificationRequestTask =
                this.reIdentificationService.ProcessReidentificationRequest(nullIdentificationRequest);

            IdentificationRequestValidationException actualIdentificationRequestValidationException =
                await Assert.ThrowsAsync<IdentificationRequestValidationException>(
                    processdentificationRequestTask.AsTask);

            // then
            actualIdentificationRequestValidationException.Should()
                .BeEquivalentTo(expectedIdentificationRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedIdentificationRequestValidationException))),
                    Times.Once());

            this.necsBrokerMock.Verify(broker =>
                broker.ReIdAsync(It.IsAny<string>()),
                    Times.Never);

            this.necsBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnProcessIfIdentificationRequestIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidIdentificationRequest = new IdentificationRequest
            {
                Identifier = invalidText,
                UserEmail = invalidText,
            };

            var invalidIdentificationRequestException =
                new InvalidIdentificationRequestException(
                    message: "Invalid identification request. Please correct the errors and try again.");

            invalidIdentificationRequestException.AddData(
                key: nameof(IdentificationRequest.UserEmail),
                values: "Text is invalid");

            invalidIdentificationRequestException.AddData(
                key: nameof(IdentificationRequest.Identifier),
                values: "Text is invalid");


            var expectedIdentificationRequestValidationException =
                new IdentificationRequestValidationException(
                    message: "Re identification validation error occurred, please fix errors and try again.",
                    innerException: invalidIdentificationRequestException);

            // when
            ValueTask<IdentificationRequest> addIdentificationRequestTask =
                this.reIdentificationService.ProcessReidentificationRequest(invalidIdentificationRequest);

            IdentificationRequestValidationException actualIdentificationRequestValidationException =
                await Assert.ThrowsAsync<IdentificationRequestValidationException>(addIdentificationRequestTask.AsTask);

            // then
            actualIdentificationRequestValidationException.Should()
                .BeEquivalentTo(expectedIdentificationRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIdentificationRequestValidationException))),
                        Times.Once);

            this.necsBrokerMock.Verify(broker =>
                broker.ReIdAsync(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.necsBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessIfIdentificationRequestHasInvalidLengthProperty()
        {
            // given
            IdentificationRequest invalidIdentificationRequest =
                CreateRandomIdentificationRequest();

            invalidIdentificationRequest.Identifier = GetRandomStringWithLength(11);
            invalidIdentificationRequest.UserEmail = GetRandomStringWithLength(321);

            var invalidIdentificationRequestException = new InvalidIdentificationRequestException(
                message: "Invalid identification request. Please correct the errors and try again.");

            invalidIdentificationRequestException.AddData(
                key: nameof(IdentificationRequest.Identifier),
                values: $"Text exceed max length of {invalidIdentificationRequest.Identifier.Length - 1} characters");

            invalidIdentificationRequestException.AddData(
                key: nameof(IdentificationRequest.UserEmail),
                values: $"Text exceed max length of {invalidIdentificationRequest.UserEmail.Length - 1} characters");


            var expectedIdentificationRequestValidationException =
                new IdentificationRequestValidationException(
                    message: "Re identification validation error occurred, please fix errors and try again.",
                    innerException: invalidIdentificationRequestException);

            // when
            ValueTask<IdentificationRequest> addIdentificationRequestTask =
                this.reIdentificationService.ProcessReidentificationRequest(invalidIdentificationRequest);

            IdentificationRequestValidationException actualIdentificationRequestValidationException =
                await Assert.ThrowsAsync<IdentificationRequestValidationException>(
                    addIdentificationRequestTask.AsTask);

            // then
            actualIdentificationRequestValidationException.Should()
                .BeEquivalentTo(expectedIdentificationRequestValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedIdentificationRequestValidationException))),
                        Times.Once);

            this.necsBrokerMock.Verify(broker =>
                broker.ReIdAsync(It.IsAny<string>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.necsBrokerMock.VerifyNoOtherCalls();
        }
    }
}
