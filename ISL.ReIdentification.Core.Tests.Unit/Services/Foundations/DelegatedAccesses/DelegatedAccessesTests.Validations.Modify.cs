// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
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

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfDelegatedAccessIsInvalidAndLogItAsync(
            string invalidText)
        {
            //given
            var invalidDelegatedAccess = new DelegatedAccess
            {
                RequesterFirstName = invalidText,
                RequesterLastName = invalidText,
                RequesterEmail = invalidText,
                RecipientFirstName = invalidText,
                RecipientLastName = invalidText,
                RecipientEmail = invalidText,
                IdentifierColumn = invalidText
            };

            var invalidDelegatedAccessException = new InvalidDelegatedAccessException(
                message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.Id),
                values: "Id is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterFirstName),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterLastName),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterEmail),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RecipientFirstName),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RecipientLastName),
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
                new[]
                    {
                        "Date is invalid",
                        $"Date is the same as {nameof(DelegatedAccess.CreatedDate)}"
                    });

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnModifyIfDelegatedAccessHasInvalidLengthPropertiesAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            var invalidDelegatedAccess = CreateRandomDelegatedAccess(dateTimeOffset: randomDateTimeOffset);
            var username = GetRandomStringWithLengthOf(256);
            invalidDelegatedAccess.RequesterFirstName = GetRandomStringWithLengthOf(256);
            invalidDelegatedAccess.RequesterLastName = GetRandomStringWithLengthOf(256);
            invalidDelegatedAccess.RequesterEmail = GetRandomStringWithLengthOf(321);
            invalidDelegatedAccess.RecipientFirstName = GetRandomStringWithLengthOf(256);
            invalidDelegatedAccess.RecipientLastName = GetRandomStringWithLengthOf(256);
            invalidDelegatedAccess.RecipientEmail = GetRandomStringWithLengthOf(321);
            invalidDelegatedAccess.IdentifierColumn = GetRandomStringWithLengthOf(11);
            invalidDelegatedAccess.CreatedBy = username;
            invalidDelegatedAccess.UpdatedBy = username;

            var invalidDelegatedAccessException =
                new InvalidDelegatedAccessException(
                    message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterFirstName),
                values: $"Text exceed max length of {invalidDelegatedAccess.RequesterFirstName.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterLastName),
                values: $"Text exceed max length of {invalidDelegatedAccess.RequesterLastName.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RequesterEmail),
                values: $"Text exceed max length of {invalidDelegatedAccess.RequesterEmail.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RecipientFirstName),
                values: $"Text exceed max length of {invalidDelegatedAccess.RecipientFirstName.Length - 1} characters");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.RecipientLastName),
                values: $"Text exceed max length of {invalidDelegatedAccess.RecipientLastName.Length - 1} characters");

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

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedDate),
                values: $"Date is the same as {nameof(DelegatedAccess.CreatedDate)}");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess(randomDateTimeOffset);
            DelegatedAccess invalidDelegatedAccess = randomDelegatedAccess;

            var invalidDelegatedAccessException = new InvalidDelegatedAccessException(
                message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedDate),
                values: $"Date is the same as {nameof(DelegatedAccess.CreatedDate)}");

            var expectedDelegatedAccessValidationException = new DelegatedAccessValidationException(
                message: "DelegatedAccess validation error occurred, please fix errors and try again.",
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
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-61)]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            //given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset now = randomDateTimeOffset;
            DateTimeOffset startDate = now.AddSeconds(-60);
            DateTimeOffset endDate = now.AddSeconds(0);
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess(randomDateTimeOffset);
            randomDelegatedAccess.UpdatedDate = randomDateTimeOffset.AddSeconds(invalidSeconds);

            var invalidDelegatedAccessException = new InvalidDelegatedAccessException(
                message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedDate),
                values:
                [
                    $"Date is not recent." +
                    $" Expected a value between {startDate} and {endDate} but found {randomDelegatedAccess.UpdatedDate}"
                ]);

            var expectedDelegatedAccessValidationException = new DelegatedAccessValidationException(
                message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                innerException: invalidDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(randomDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageDelegatedAccessDoesNotExistAndLogItAsync()
        {
            //given
            int randomNegative = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess(randomDateTimeOffset);
            DelegatedAccess nonExistingDelegatedAccess = randomDelegatedAccess;
            nonExistingDelegatedAccess.CreatedDate = randomDateTimeOffset.AddMinutes(randomNegative);
            DelegatedAccess nullDelegatedAccess = null;

            var notFoundDelegatedAccessException =
                new NotFoundDelegatedAccessException(
                    message: $"DelegatedAccess not found with id: {nonExistingDelegatedAccess.Id}");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: notFoundDelegatedAccessException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(nonExistingDelegatedAccess.Id))
                    .ReturnsAsync(nullDelegatedAccess);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(nonExistingDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(nonExistingDelegatedAccess.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessValidationException))),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedAuditInfoHasChangedAndLogItAsync()
        {
            //given
            int randomMinutes = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DelegatedAccess randomDelegatedAccess = CreateRandomModifyDelegatedAccess(randomDateTimeOffset);
            DelegatedAccess invalidDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess storedDelegatedAccess = randomDelegatedAccess.DeepClone();
            storedDelegatedAccess.CreatedBy = GetRandomString();
            storedDelegatedAccess.CreatedDate = storedDelegatedAccess.CreatedDate.AddMinutes(randomMinutes);
            storedDelegatedAccess.UpdatedDate = storedDelegatedAccess.UpdatedDate.AddMinutes(randomMinutes);
            Guid DelegatedAccessId = invalidDelegatedAccess.Id;

            var invalidDelegatedAccessException = new InvalidDelegatedAccessException(
                message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.CreatedBy),
                values: $"Text is not the same as {nameof(DelegatedAccess.CreatedBy)}");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.CreatedDate),
                values: $"Date is not the same as {nameof(DelegatedAccess.CreatedDate)}");

            var expectedDelegatedAccessValidationException = new DelegatedAccessValidationException(
                message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                innerException: invalidDelegatedAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(DelegatedAccessId))
                    .ReturnsAsync(storedDelegatedAccess);

            // when
            ValueTask<DelegatedAccess> modifyDelegatedAccessTask =
                this.delegatedAccessService.ModifyDelegatedAccessAsync(invalidDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(
                    modifyDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(invalidDelegatedAccess.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(
                    SameExceptionAs(expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DelegatedAccess randomDelegatedAccess = CreateRandomModifyDelegatedAccess(randomDateTimeOffset);
            DelegatedAccess invalidDelegatedAccess = randomDelegatedAccess;

            DelegatedAccess storageDelegatedAccess = randomDelegatedAccess.DeepClone();
            invalidDelegatedAccess.UpdatedDate = storageDelegatedAccess.UpdatedDate;

            var invalidDelegatedAccessException = new InvalidDelegatedAccessException(
                message: "Invalid delegated access. Please correct the errors and try again.");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedDate),
                values: $"Date is the same as {nameof(DelegatedAccess.UpdatedDate)}");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: invalidDelegatedAccessException);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(invalidDelegatedAccess.Id))
                .ReturnsAsync(storageDelegatedAccess);

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
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(
                expectedDelegatedAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(invalidDelegatedAccess.Id),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
