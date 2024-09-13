using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfLookupIsNullAndLogItAsync()
        {
            // given
            Lookup nullLookup = null;

            var nullLookupException =
                new NullLookupException(message: "Lookup is null.");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: nullLookupException);

            // when
            ValueTask<Lookup> addLookupTask =
                this.lookupService.AddLookupAsync(nullLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    addLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfLookupIsInvalidAndLogItAsync(string invalidText)
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
                values: "Date is required");

            invalidLookupException.AddData(
                key: nameof(Lookup.UpdatedBy),
                values: "Text is required");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            // when
            ValueTask<Lookup> addLookupTask =
                this.lookupService.AddLookupAsync(invalidLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    addLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLookupAsync(It.IsAny<Lookup>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomLookup(randomDateTimeOffset);
            Lookup invalidLookup = randomLookup;

            invalidLookup.UpdatedDate =
                invalidLookup.CreatedDate.AddDays(randomNumber);

            var invalidLookupException = 
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.UpdatedDate),
                values: $"Date is not the same as {nameof(Lookup.CreatedDate)}");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            // when
            ValueTask<Lookup> addLookupTask =
                this.lookupService.AddLookupAsync(invalidLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    addLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLookupAsync(It.IsAny<Lookup>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateUsersIsNotSameAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Lookup randomLookup = CreateRandomLookup(randomDateTimeOffset);
            Lookup invalidLookup = randomLookup;
            invalidLookup.UpdatedBy = Guid.NewGuid().ToString();

            var invalidLookupException =
                new InvalidLookupException(
                    message: "Invalid lookup. Please correct the errors and try again.");

            invalidLookupException.AddData(
                key: nameof(Lookup.UpdatedBy),
                values: $"Text is not the same as {nameof(Lookup.CreatedBy)}");

            var expectedLookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: invalidLookupException);

            // when
            ValueTask<Lookup> addLookupTask =
                this.lookupService.AddLookupAsync(invalidLookup);

            LookupValidationException actualLookupValidationException =
                await Assert.ThrowsAsync<LookupValidationException>(
                    addLookupTask.AsTask);

            // then
            actualLookupValidationException.Should()
                .BeEquivalentTo(expectedLookupValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLookupValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertLookupAsync(It.IsAny<Lookup>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}