// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.AccessAudits
{
    public partial class AccessAuditTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddAccessAuditAsync()
        {
            // given
            AccessAudit nullAccessAudit = null;
            var nullAccessAuditException = new NullAccessAuditException(message: "Access audit is null.");

            var expectedAccessAuditValidationException =
                new AccessAuditValidationException(
                    message: "Access audit validation error occurred, please fix errors and try again.",
                    innerException: nullAccessAuditException);

            // when
            ValueTask<AccessAudit> addAccessAuditTask = this.accessAuditService.AddAccessAuditAsync(nullAccessAudit);

            AccessAuditValidationException actualAccessAuditValidationException =
                await Assert.ThrowsAsync<AccessAuditValidationException>(addAccessAuditTask.AsTask);

            // then
            actualAccessAuditValidationException.Should().BeEquivalentTo(expectedAccessAuditValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedAccessAuditValidationException))), Times.Once());

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertAccessAuditAsync(It.IsAny<AccessAudit>()),
                    Times.Never);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfAccessAuditIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidAccessAudit = new AccessAudit
            {
                PseudoIdentifier = invalidText,
                UserEmail = invalidText,
            };

            var invalidAccessAuditException =
                new InvalidAccessAuditException(
                    message: "Invalid access audit. Please correct the errors and try again.");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.Id),
                values: "Id is invalid");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.UserEmail),
                values: "Text is invalid");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.PseudoIdentifier),
                values: "Text is invalid");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.CreatedDate),
                values: "Date is invalid");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.CreatedBy),
                values: "Text is invalid");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.UpdatedDate),
                values: "Date is invalid");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.UpdatedBy),
                values: "Text is invalid");

            var expectedAccessAuditValidationException =
                new AccessAuditValidationException(
                    message: "Access audit validation error occurred, please fix errors and try again.",
                    innerException: invalidAccessAuditException);

            // when
            ValueTask<AccessAudit> addAccessAuditTask =
                this.accessAuditService.AddAccessAuditAsync(invalidAccessAudit);

            AccessAuditValidationException actualAccessAuditValidationException =
                await Assert.ThrowsAsync<AccessAuditValidationException>(addAccessAuditTask.AsTask);

            // then
            actualAccessAuditValidationException.Should()
                .BeEquivalentTo(expectedAccessAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAccessAuditValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertAccessAuditAsync(It.IsAny<AccessAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfAccessAuditHasInvalidLengthProperty()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            AccessAudit invalidAccessAudit = CreateRandomAccessAudit(dateTimeOffset: randomDateTimeOffset);
            var inputCreatedByUpdatedByString = GetRandomStringWithLength(256);
            invalidAccessAudit.UserEmail = GetRandomStringWithLength(321);
            invalidAccessAudit.PseudoIdentifier = GetRandomStringWithLength(10);
            invalidAccessAudit.CreatedBy = inputCreatedByUpdatedByString;
            invalidAccessAudit.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidAccessAuditException = new InvalidAccessAuditException(
                message: "Invalid access audit. Please correct the errors and try again.");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.PseudoIdentifier),
                values: $"Text exceed max length of {invalidAccessAudit.PseudoIdentifier.Length - 1} characters");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.UserEmail),
                values: $"Text exceed max length of {invalidAccessAudit.UserEmail.Length - 1} characters");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.CreatedBy),
                values: $"Text exceed max length of {invalidAccessAudit.CreatedBy.Length - 1} characters");

            invalidAccessAuditException.AddData(
                key: nameof(AccessAudit.UpdatedBy),
                values: $"Text exceed max length of {invalidAccessAudit.UpdatedBy.Length - 1} characters");

            var expectedAccessAuditValidationException =
                new AccessAuditValidationException(
                    message: "Access audit validation error occurred, please fix errors and try again.",
                    innerException: invalidAccessAuditException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<AccessAudit> addAccessAuditTask =
                this.accessAuditService.AddAccessAuditAsync(invalidAccessAudit);

            AccessAuditValidationException actualAccessAuditValidationException =
                await Assert.ThrowsAsync<AccessAuditValidationException>(
                    addAccessAuditTask.AsTask);

            // then
            actualAccessAuditValidationException.Should()
                .BeEquivalentTo(expectedAccessAuditValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAccessAuditValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertAccessAuditAsync(It.IsAny<AccessAudit>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
