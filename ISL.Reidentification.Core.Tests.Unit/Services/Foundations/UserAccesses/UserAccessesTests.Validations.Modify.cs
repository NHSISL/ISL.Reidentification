// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserAccessIsNullAndLogItAsync()
        {
            // given
            UserAccess nullUserAccess = null;
            var nullUserAccessException = new NullUserAccessException(message: "User access is null.");

            var expectedUserAccessValidationException =
                new UserAccessValidationException(
                    message: "UserAccess validation error occurred, please fix errors and try again.",
                    innerException: nullUserAccessException);

            // when
            ValueTask<UserAccess> modifyUserAccessTask = this.userAccessService.ModifyUserAccessAsync(nullUserAccess);

            UserAccessValidationException actualUserAccessValidationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessValidationException.Should().BeEquivalentTo(expectedUserAccessValidationException);
            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedUserAccessValidationException))), Times.Once());

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(It.IsAny<UserAccess>()),
                    Times.Never);

            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserAccessIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidUserAccess = new UserAccess
            {
                UserEmail = invalidText,
                RecipientEmail = invalidText,
                OrgCode = invalidText,
            };

            var invalidUserAccessException =
                new InvalidUserAccessException(
                    message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.Id),
                values: "Id is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.UserEmail),
                values: "Text is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.RecipientEmail),
                values: "Text is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.OrgCode),
                values: "Text is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.ActiveFrom),
                values: "Date is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.CreatedDate),
                values: "Date is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.CreatedBy),
                values: "Text is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.UpdatedDate),
                values:
                    new[]
                    {
                        "Date is invalid",
                        $"Date is the same as {nameof(UserAccess.CreatedDate)}"
                    });

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.UpdatedBy),
                values: "Text is invalid");

            var expectedUserAccessValidationException =
                new UserAccessValidationException(
                    message: "UserAccess validation error occurred, please fix errors and try again.",
                    innerException: invalidUserAccessException);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(invalidUserAccess);

            UserAccessValidationException actualUserAccessValidationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessValidationException.Should()
                .BeEquivalentTo(expectedUserAccessValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedUserAccessValidationException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(It.IsAny<UserAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserAccessHasInvalidLengthProperty()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            UserAccess invalidUserAccess = CreateRandomModifyUserAccess(randomDateTimeOffset);
            var inputCreatedByUpdatedByString = GetRandomStringWithLength(256);
            invalidUserAccess.UserEmail = GetRandomStringWithLength(321);
            invalidUserAccess.RecipientEmail = GetRandomStringWithLength(321);
            invalidUserAccess.OrgCode = GetRandomStringWithLength(16);
            invalidUserAccess.CreatedBy = inputCreatedByUpdatedByString;
            invalidUserAccess.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidUserAccessException = new InvalidUserAccessException(
                message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.UserEmail),
                values: $"Text exceed max length of {invalidUserAccess.UserEmail.Length - 1} characters");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.RecipientEmail),
                values: $"Text exceed max length of {invalidUserAccess.RecipientEmail.Length - 1} characters");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.OrgCode),
                values: $"Text exceed max length of {invalidUserAccess.OrgCode.Length - 1} characters");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.CreatedBy),
                values: $"Text exceed max length of {invalidUserAccess.CreatedBy.Length - 1} characters");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.UpdatedBy),
                values: $"Text exceed max length of {invalidUserAccess.UpdatedBy.Length - 1} characters");

            var expectedUserAccessException = new
                UserAccessValidationException(
                    message: "UserAccess validation error occurred, please fix errors and try again.",
                    innerException: invalidUserAccessException);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(invalidUserAccess);

            UserAccessValidationException actualUserAccessValidationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessValidationException.Should().BeEquivalentTo(expectedUserAccessException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedUserAccessException))),
                        Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(It.IsAny<UserAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModiyIfUserAccessHasSameCreatedDateUpdatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDatTimeOffset = GetRandomDateTimeOffset();
            UserAccess randomUserAccess = CreateRandomUserAccess(randomDatTimeOffset);
            var invalidUserAccess = randomUserAccess;

            var invalidUserAccessException = new InvalidUserAccessException(
                message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.UpdatedDate),
                values: $"Date is the same as {nameof(UserAccess.CreatedDate)}");

            var expectedUserAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: invalidUserAccessException);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(invalidUserAccess);

            UserAccessValidationException actualUserAccessVaildationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessVaildationException.Should().BeEquivalentTo(expectedUserAccessValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(
                   SameExceptionAs(expectedUserAccessValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-61)]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
            int invalidSeconds)
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset now = randomDateTimeOffset;
            DateTimeOffset startDate = now.AddSeconds(-60);
            DateTimeOffset endDate = now.AddSeconds(0);
            UserAccess randomUserAccess = CreateRandomUserAccess(randomDateTimeOffset);
            UserAccess invalidUserAccess = randomUserAccess;
            invalidUserAccess.UpdatedDate = randomDateTimeOffset.AddSeconds(invalidSeconds);

            var invalidUserAccessException = new InvalidUserAccessException(
                message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.UpdatedDate),
                values:
                [
                    $"Date is not recent." +
                    $" Expected a value between {startDate} and {endDate} but found {randomUserAccess.UpdatedDate}"
                ]);

            var expectedUserAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: invalidUserAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(invalidUserAccess);

            UserAccessValidationException actualUserAccessVaildationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessVaildationException.Should().BeEquivalentTo(expectedUserAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(
                   SameExceptionAs(expectedUserAccessValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUserAccessDoesNotExistAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            UserAccess randomUserAccess = CreateRandomUserAccess(randomDateTimeOffset);
            UserAccess nonExistingUserAccess = randomUserAccess;
            nonExistingUserAccess.CreatedDate = randomDateTimeOffset.AddMinutes(randomNegativeNumber);
            UserAccess nullUserAccess = null;

            var notFoundUserAccessException = new NotFoundUserAccessException(
                message: $"User access not found with id: {nonExistingUserAccess.Id}");

            var expectedUserAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: notFoundUserAccessException);

            this.reidentificationStorageBroker.Setup(broker =>
                broker.SelectUserAccessByIdAsync(nonExistingUserAccess.Id))
                    .ReturnsAsync(nullUserAccess);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(nonExistingUserAccess);

            UserAccessValidationException actualUserAccessVaildationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessVaildationException.Should().BeEquivalentTo(expectedUserAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.SelectUserAccessByIdAsync(nonExistingUserAccess.Id),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(
                   SameExceptionAs(expectedUserAccessValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reidentificationStorageBroker.VerifyNoOtherCalls();
        }
    }
}
