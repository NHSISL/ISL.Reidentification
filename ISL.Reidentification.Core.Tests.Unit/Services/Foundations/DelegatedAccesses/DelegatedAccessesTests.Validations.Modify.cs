// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDelegatedAccessIsNullAndLogItAsync()
        {
            //given
            DelegatedAccess nullDelegatedAccess = null;

            var nullDelegatedAccessException =
                new NullDelegatedAccessException(message: "Delegated access is null.");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: nullDelegatedAccessException);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(nullDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfDelegatedAccessIsInvalidAndLogItAsync(string invalidString)
        {
            //given
            var invalidDelegatedAccess = new DelegatedAccess
            {
                RequesterEmail = invalidString,
                RecipientEmail = invalidString,
                IdentifierColumn = invalidString,
            };

            var invalidDelegatedAccessException = new InvalidDelegatedAccessException(
                message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.Id),
                values: "Id is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterEmail),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RecipientEmail),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.IdentifierColumn),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.CreatedBy),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedBy),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.CreatedDate),
                values: "Date is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedDate),
                values: "Date is invalid");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: invalidDelegatedAccessException);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(invalidDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffsetAsync(),
            //        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfDelegatedAccessHasInvalidLengthPropertiesAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            var invalidDelegatedAccess = CreateRandomDelegatedAccess(dateTimeOffset: randomDateTimeOffset);
            var email = GetRandomStringWithLengthOf(321);
            var identifierColumn = GetRandomStringWithLengthOf(51);
            var username = GetRandomStringWithLengthOf(256);
            invalidDelegatedAccess.RequesterEmail = email;
            invalidDelegatedAccess.RecipientEmail = email;
            invalidDelegatedAccess.IdentifierColumn = identifierColumn;
            invalidDelegatedAccess.CreatedBy = username;
            invalidDelegatedAccess.UpdatedBy = username;

            var invalidDelegatedAccessException =
                new InvalidDelegatedAccessException(
                    message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterEmail),
                values: $"Text exceed max length of {invalidDelegatedAccess.RequesterEmail.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RecipientEmail),
                values: $"Text exceed max length of {invalidDelegatedAccess.RecipientEmail.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.IdentifierColumn),
                values: $"Text exceed max length of {invalidDelegatedAccess.IdentifierColumn.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.CreatedBy),
                values: $"Text exceed max length of {invalidDelegatedAccess.CreatedBy.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedBy),
                values: $"Text exceed max length of {invalidDelegatedAccess.UpdatedBy.Length - 1} characters");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, fix errors and try again.",
                    innerException: invalidDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(invalidDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should()
                .BeEquivalentTo(expectedDelegatedAccessValidationException);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffsetAsync(),
            //        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }
    }
}
