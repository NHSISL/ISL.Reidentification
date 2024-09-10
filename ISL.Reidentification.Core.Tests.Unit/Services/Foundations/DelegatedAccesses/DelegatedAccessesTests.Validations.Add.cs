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
        public async Task ShouldThrowValidationExceptionOnAddDelegatedAccessAsync()
        {
            //given
            DelegatedAccess nullDelegatedAccess = null;
            var nullDelegatedAccessException = new NullDelegatedAccessException(message: "Delegated access is null.");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: nullDelegatedAccessException);

            //when
            ValueTask<DelegatedAccess> addDelegatedAccessTask = this.delegatedAccessService.AddDelegatedAccessAsync(nullDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(addDelegatedAccessTask.AsTask);

            //then
            actualDelegatedAccessValidationException.Should().BeEquivalentTo(expectedDelegatedAccessValidationException);
            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedDelegatedAccessValidationException))), Times.Once());

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfDelegatedAccessIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidDelegatedAccess = new DelegatedAccess
            {
                RequesterEmail = invalidText,
                RecipientEmail = invalidText,
                IdentifierColumn = invalidText
            };

            var invalidDelegatedAccessException =
                new InvalidDelegatedAccessException(
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
                key: nameof(DelegatedAccess.CreatedDate),
                values: "Date is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.CreatedBy),
                values: "Text is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedDate),
                values: "Date is invalid");

            invalidDelegatedAccessException.AddData(
                key: nameof(DelegatedAccess.UpdatedBy),
                values: "Text is invalid");

            var expectedDelegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                    innerException: invalidDelegatedAccessException);

            // when
            ValueTask<DelegatedAccess> addDelegatedAccessTask =
                this.delegatedAccessService.AddDelegatedAccessAsync(invalidDelegatedAccess);

            DelegatedAccessValidationException actualDelegatedAccessValidationException =
                await Assert.ThrowsAsync<DelegatedAccessValidationException>(addDelegatedAccessTask.AsTask);

            // then
            actualDelegatedAccessValidationException.Should()
                .BeEquivalentTo(expectedDelegatedAccessValidationException);

            //this.dateTimeBrokerMock.Verify(broker =>
            //    broker.GetCurrentDateTimeOffsetAsync(),
            //        Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedDelegatedAccessValidationException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
