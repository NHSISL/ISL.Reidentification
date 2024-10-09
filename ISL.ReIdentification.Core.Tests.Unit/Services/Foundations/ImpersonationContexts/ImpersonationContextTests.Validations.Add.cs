// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ImpersonationContexts
{
    public partial class ImpersonationContextsTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddImpersonationContextAsync()
        {
            //given
            ImpersonationContext nullImpersonationContext = null;
            var nullImpersonationContextException = new NullImpersonationContextException(message: "Impersonation context is null.");

            var expectedImpersonationContextValidationException =
                new ImpersonationContextValidationException(
                    message: "ImpersonationContext validation error occurred, please fix errors and try again.",
                    innerException: nullImpersonationContextException);

            //when
            ValueTask<ImpersonationContext> addImpersonationContextTask =
                this.impersonationContextService.AddImpersonationContextAsync(nullImpersonationContext);

            ImpersonationContextValidationException actualImpersonationContextValidationException =
                await Assert.ThrowsAsync<ImpersonationContextValidationException>(addImpersonationContextTask.AsTask);

            //then
            actualImpersonationContextValidationException.Should()
                .BeEquivalentTo(expectedImpersonationContextValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedImpersonationContextValidationException))), Times.Once());

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Never);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfImpersonationContextIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidImpersonationContext = new ImpersonationContext
            {
                RequesterFirstName = invalidText,
                RequesterLastName = invalidText,
                RequesterEmail = invalidText,
                RecipientFirstName = invalidText,
                RecipientLastName = invalidText,
                RecipientEmail = invalidText,
                IdentifierColumn = invalidText
            };

            var invalidImpersonationContextException =
                new InvalidImpersonationContextException(
                    message: "Invalid impersonation context. Please correct the errors and try again.");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.Id),
                values: "Id is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RequesterFirstName),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RequesterLastName),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RequesterEmail),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RecipientFirstName),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RecipientLastName),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RecipientEmail),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.IdentifierColumn),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.CreatedDate),
                values: "Date is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.CreatedBy),
                values: "Text is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.UpdatedDate),
                values: "Date is invalid");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.UpdatedBy),
                values: "Text is invalid");

            var expectedImpersonationContextValidationException =
                new ImpersonationContextValidationException(
                    message: "ImpersonationContext validation error occurred, please fix errors and try again.",
                    innerException: invalidImpersonationContextException);

            // when
            ValueTask<ImpersonationContext> addImpersonationContextTask =
                this.impersonationContextService.AddImpersonationContextAsync(invalidImpersonationContext);

            ImpersonationContextValidationException actualImpersonationContextValidationException =
                await Assert.ThrowsAsync<ImpersonationContextValidationException>(addImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextValidationException.Should()
                .BeEquivalentTo(expectedImpersonationContextValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedImpersonationContextValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfImpersonationContextHasInvalidLengthPropertiesAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            var invalidImpersonationContext = CreateRandomImpersonationContext(dateTimeOffset: randomDateTimeOffset);
            var username = GetRandomStringWithLengthOf(256);
            invalidImpersonationContext.RequesterFirstName = GetRandomStringWithLengthOf(256);
            invalidImpersonationContext.RequesterLastName = GetRandomStringWithLengthOf(256);
            invalidImpersonationContext.RequesterEmail = GetRandomStringWithLengthOf(321);
            invalidImpersonationContext.RecipientFirstName = GetRandomStringWithLengthOf(256);
            invalidImpersonationContext.RecipientLastName = GetRandomStringWithLengthOf(256);
            invalidImpersonationContext.RecipientEmail = GetRandomStringWithLengthOf(321);
            invalidImpersonationContext.IdentifierColumn = GetRandomStringWithLengthOf(11);
            invalidImpersonationContext.CreatedBy = username;
            invalidImpersonationContext.UpdatedBy = username;

            var invalidImpersonationContextException =
                new InvalidImpersonationContextException(
                    message: "Invalid impersonation context. Please correct the errors and try again.");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RequesterFirstName),
                values: $"Text exceed max length of {invalidImpersonationContext.RequesterFirstName.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RequesterLastName),
                values: $"Text exceed max length of {invalidImpersonationContext.RequesterLastName.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RequesterEmail),
                values: $"Text exceed max length of {invalidImpersonationContext.RequesterEmail.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RecipientFirstName),
                values: $"Text exceed max length of {invalidImpersonationContext.RecipientFirstName.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RecipientLastName),
                values: $"Text exceed max length of {invalidImpersonationContext.RecipientLastName.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.RecipientEmail),
                values: $"Text exceed max length of {invalidImpersonationContext.RecipientEmail.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.IdentifierColumn),
                values: $"Text exceed max length of {invalidImpersonationContext.IdentifierColumn.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.CreatedBy),
                values: $"Text exceed max length of {invalidImpersonationContext.CreatedBy.Length - 1} characters");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.UpdatedBy),
                values: $"Text exceed max length of {invalidImpersonationContext.UpdatedBy.Length - 1} characters");

            var expectedImpersonationContextValidationException =
                new ImpersonationContextValidationException(
                    message: "ImpersonationContext validation error occurred, please fix errors and try again.",
                    innerException: invalidImpersonationContextException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<ImpersonationContext> addImpersonationContextTask =
                this.impersonationContextService.AddImpersonationContextAsync(invalidImpersonationContext);

            ImpersonationContextValidationException actualImpersonationContextValidationException =
                await Assert.ThrowsAsync<ImpersonationContextValidationException>(
                    addImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextValidationException.Should()
                .BeEquivalentTo(expectedImpersonationContextValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedImpersonationContextValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAuditPropertiesIsNotTheSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            DateTimeOffset now = randomDateTime;
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext(now);
            ImpersonationContext invalidImpersonationContext = randomImpersonationContext;
            invalidImpersonationContext.CreatedBy = GetRandomString();
            invalidImpersonationContext.UpdatedBy = GetRandomString();
            invalidImpersonationContext.CreatedDate = now;
            invalidImpersonationContext.UpdatedDate = GetRandomDateTimeOffset();

            var invalidImpersonationContextException = new InvalidImpersonationContextException(
                message: "Invalid impersonation context. Please correct the errors and try again.");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.UpdatedBy),
                values: $"Text is not the same as {nameof(ImpersonationContext.CreatedBy)}");

            invalidImpersonationContextException.AddData(
                key: nameof(ImpersonationContext.UpdatedDate),
                values: $"Date is not the same as {nameof(ImpersonationContext.CreatedDate)}");

            var expectedImpersonationContextValidationException =
                new ImpersonationContextValidationException(
                    message: "ImpersonationContext validation error occurred, please fix errors and try again.",
                    innerException: invalidImpersonationContextException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(now);

            // when
            ValueTask<ImpersonationContext> addImpersonationContextTask =
                this.impersonationContextService.AddImpersonationContextAsync(invalidImpersonationContext);

            ImpersonationContextValidationException actualImpersonationContextValidationException =
                await Assert.ThrowsAsync<ImpersonationContextValidationException>(
                    addImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextValidationException.Should().BeEquivalentTo(
                expectedImpersonationContextValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedImpersonationContextValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-61)]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            // given
            DateTimeOffset randomDateTime =
                GetRandomDateTimeOffset();

            DateTimeOffset now = randomDateTime;
            DateTimeOffset startDate = now.AddSeconds(-60);
            DateTimeOffset endDate = now.AddSeconds(0);
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext();
            ImpersonationContext invalidImpersonationContext = randomImpersonationContext;

            DateTimeOffset invalidDate =
                now.AddSeconds(invalidSeconds);

            invalidImpersonationContext.CreatedDate = invalidDate;
            invalidImpersonationContext.UpdatedDate = invalidDate;

            var invalidImpersonationContextException = new InvalidImpersonationContextException(
                message: "Invalid impersonation context. Please correct the errors and try again.");

            invalidImpersonationContextException.AddData(
            key: nameof(ImpersonationContext.CreatedDate),
                values:
                    $"Date is not recent. Expected a value between " +
                    $"{startDate} and {endDate} but found {invalidDate}");

            var expectedImpersonationContextValidationException =
                new ImpersonationContextValidationException(
                    message: "ImpersonationContext validation error occurred, please fix errors and try again.",
                    innerException: invalidImpersonationContextException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(now);

            // when
            ValueTask<ImpersonationContext> addImpersonationContextTask =
                this.impersonationContextService.AddImpersonationContextAsync(invalidImpersonationContext);

            ImpersonationContextValidationException actualImpersonationContextValidationException =
                await Assert.ThrowsAsync<ImpersonationContextValidationException>(
                    addImpersonationContextTask.AsTask);

            // then
            actualImpersonationContextValidationException.Should().BeEquivalentTo(
                expectedImpersonationContextValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedImpersonationContextValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }
    }
}
