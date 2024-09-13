using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLookupIsNullAndLogItAsync()
        {
            // given
            Lookup nullLookup = null;
            var nullLookupException = new NullLookupException(message: "Lookup is null.");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: nullLookupException);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(nullLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(It.IsAny<Lookup>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfLookupIsInvalidAndLogItAsync(string invalidText)
        {
            // given 
            var invalidLookup = new Lookup
            {
                // TODO:  Add default values for your properties i.e. Name = invalidText
            };

            var invalidLookupException = 
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.Id),
                values: "Id is required");

            //invalidLookupException.AddData(
            //    key: nameof(Lookup.Name),
            //    values: "Text is required");

            // TODO: Add or remove data here to suit the validation needs for the Lookup model

            invalidLookupException.AddData(
                key: nameof(Lookup.CreatedDate),
                values: "Date is required");

            invalidLookupException.AddData(
                key: nameof(Lookup.CreatedBy),
                values: "Text is required");

            invalidLookupException.AddData(
                key: nameof(Lookup.UpdatedDate),
                values:
                new[] {
                    "Date is required",
                    $"Date is the same as {nameof(Lookup.CreatedDate)}"
                });

            invalidLookupException.AddData(
                key: nameof(Lookup.UpdatedBy),
                values: "Text is required");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(invalidLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    modifyLookupTask.AsTask);

            //then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once());

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateLookupAsync(It.IsAny<Lookup>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomLookup(randomDateTimeOffset);
            Lookup invalidLookup = randomLookup;
            
            var invalidLookupException = 
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.UpdatedDate),
                values: $"Date is the same as {nameof(Lookup.CreatedDate)}");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(invalidLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(invalidLookup.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(int minutes)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomLookup(randomDateTimeOffset);
            randomLookup.UpdatedDate = randomDateTimeOffset.AddMinutes(minutes);

            var invalidLookupException = 
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.UpdatedDate),
                values: "Date is not recent");

            var expectedLookupValidatonException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(randomLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidatonException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidatonException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfLookupDoesNotExistAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomModifyLookup(randomDateTimeOffset);
            Lookup nonExistLookup = randomLookup;
            Lookup nullLookup = null;

            var notFoundLookupException =
                new NotFoundLookupException(nonExistLookup.Id);

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: notFoundLookupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(nonExistLookup.Id))
                .ReturnsAsync(nullLookup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when 
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(nonExistLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(nonExistLookup.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomModifyLookup(randomDateTimeOffset);
            Lookup invalidLookup = randomLookup.DeepClone();
            Lookup storageLookup = invalidLookup.DeepClone();
            storageLookup.CreatedDate = storageLookup.CreatedDate.AddMinutes(randomMinutes);
            storageLookup.UpdatedDate = storageLookup.UpdatedDate.AddMinutes(randomMinutes);
            
            var invalidLookupException = 
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.CreatedDate),
                values: $"Date is not the same as {nameof(Lookup.CreatedDate)}");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(invalidLookup.Id))
                .ReturnsAsync(storageLookup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(invalidLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(invalidLookup.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedLookupValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedUserDontMacthStorageAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomModifyLookup(randomDateTimeOffset);
            Lookup invalidLookup = randomLookup.DeepClone();
            Lookup storageLookup = invalidLookup.DeepClone();
            invalidLookup.CreatedBy = Guid.NewGuid().ToString();
            storageLookup.UpdatedDate = storageLookup.CreatedDate;

            var invalidLookupException = 
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.CreatedBy),
                values: $"Text is not the same as {nameof(Lookup.CreatedBy)}");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectLookupByIdAsync(invalidLookup.Id))
                .ReturnsAsync(storageLookup);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                .Returns(randomDateTimeOffset);

            // when
            ValueTask<Lookup> modifyLookupTask =
                this.lookupService.ModifyLookupAsync(invalidLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    modifyLookupTask.AsTask);

            // then
            actualLookupValidationException.Should().BeEquivalentTo(expectedLookupValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectLookupByIdAsync(invalidLookup.Id),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedLookupValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}