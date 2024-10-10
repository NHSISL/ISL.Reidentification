﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.UserAccesses
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

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(It.IsAny<UserAccess>()),
                    Times.Never);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
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
                EntraUserId = Guid.Empty,
                Email = invalidText,
                OrgCode = invalidText,
            };

            var invalidUserAccessException =
                new InvalidUserAccessException(
                    message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.Id),
                values: "Id is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.EntraUserId),
                values: "Id is invalid");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.Email),
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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedUserAccessValidationException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(It.IsAny<UserAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserAccessHasInvalidLengthProperty()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            UserAccess invalidUserAccess = CreateRandomModifyUserAccess(randomDateTimeOffset);
            var inputCreatedByUpdatedByString = GetRandomStringWithLength(256);
            invalidUserAccess.GivenName = GetRandomStringWithLength(256);
            invalidUserAccess.Surname = GetRandomStringWithLength(256);
            invalidUserAccess.Email = GetRandomStringWithLength(321);
            invalidUserAccess.OrgCode = GetRandomStringWithLength(16);
            invalidUserAccess.CreatedBy = inputCreatedByUpdatedByString;
            invalidUserAccess.UpdatedBy = inputCreatedByUpdatedByString;

            var invalidUserAccessException = new InvalidUserAccessException(
                message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.GivenName),
                values: $"Text exceed max length of {invalidUserAccess.GivenName.Length - 1} characters");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.Surname),
                values: $"Text exceed max length of {invalidUserAccess.Surname.Length - 1} characters");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.Email),
                values: $"Text exceed max length of {invalidUserAccess.Email.Length - 1} characters");

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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedUserAccessException))),
                        Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertUserAccessAsync(It.IsAny<UserAccess>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
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

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(
                   SameExceptionAs(expectedUserAccessValidationException))),
                       Times.Once);

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
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
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
                message: $"User access not found with Id: {nonExistingUserAccess.Id}");

            var expectedUserAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: notFoundUserAccessException);

            this.reIdentificationStorageBroker.Setup(broker =>
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

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectUserAccessByIdAsync(nonExistingUserAccess.Id),
                Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(
                   SameExceptionAs(expectedUserAccessValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionIfStorageUserAccessCreatedDateIsNotSameAsUserAccessCreatedDateAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetRandomNegativeNumber();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            UserAccess randomUserAccess = CreateRandomModifyUserAccess(randomDateTimeOffset);
            UserAccess invalidUserAccess = randomUserAccess;
            UserAccess storageUserAccess = invalidUserAccess.DeepClone();
            storageUserAccess.CreatedDate = randomDateTimeOffset.AddMinutes(randomNegativeNumber);
            storageUserAccess.UpdatedDate = randomDateTimeOffset.AddMinutes(randomNegativeNumber);

            var invalidUserAccessException = new InvalidUserAccessException(
                message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessException.AddData(
                key: nameof(UserAccess.CreatedDate),
                values: $"Date is not the same as {nameof(UserAccess.CreatedDate)}");

            var expectedUserAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: invalidUserAccessException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectUserAccessByIdAsync(invalidUserAccess.Id))
                    .ReturnsAsync(storageUserAccess);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(invalidUserAccess);

            UserAccessValidationException actualUserAccessValidationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessValidationException.Should().BeEquivalentTo(expectedUserAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectUserAccessByIdAsync(invalidUserAccess.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedUserAccessValidationException))),
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
            UserAccess randomUserAccess = CreateRandomModifyUserAccess(randomDateTimeOffset);
            UserAccess invalidUserAccess = randomUserAccess;
            UserAccess storageUserAccess = invalidUserAccess.DeepClone();
            invalidUserAccess.UpdatedDate = storageUserAccess.UpdatedDate;

            var invalidUserAccessValidationException = new InvalidUserAccessException(
                message: "Invalid user access. Please correct the errors and try again.");

            invalidUserAccessValidationException.AddData(
                key: nameof(UserAccess.UpdatedDate),
                values: $"Date is the same as {nameof(UserAccess.UpdatedDate)}");

            var expectedUserAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: invalidUserAccessValidationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectUserAccessByIdAsync(invalidUserAccess.Id))
                    .ReturnsAsync(storageUserAccess);

            // when
            ValueTask<UserAccess> modifyUserAccessTask =
                this.userAccessService.ModifyUserAccessAsync(invalidUserAccess);

            UserAccessValidationException actualUserAccessValidationException =
                await Assert.ThrowsAsync<UserAccessValidationException>(modifyUserAccessTask.AsTask);

            // then
            actualUserAccessValidationException.Should().BeEquivalentTo(expectedUserAccessValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectUserAccessByIdAsync(invalidUserAccess.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(expectedUserAccessValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
