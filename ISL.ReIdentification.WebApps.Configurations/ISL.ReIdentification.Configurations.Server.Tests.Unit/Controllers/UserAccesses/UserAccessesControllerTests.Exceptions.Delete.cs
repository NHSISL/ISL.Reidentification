﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.UserAccesses
{
    public partial class UserAccessesControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<UserAccess>(expectedBadRequestObjectResult);

            this.userAccessServiceMock.Setup(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<UserAccess> actualActionResult =
                await this.userAccessesController.DeleteUserAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.userAccessServiceMock.Verify(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnDeleteIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<UserAccess>(expectedBadRequestObjectResult);

            this.userAccessServiceMock.Setup(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<UserAccess> actualActionResult =
                await this.userAccessesController.DeleteUserAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.userAccessServiceMock.Verify(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundUserAccessException =
                new NotFoundUserAccessException(
                    message: someMessage);

            var userAccessValidationException =
                new UserAccessValidationException(
                    message: someMessage,
                    innerException: notFoundUserAccessException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundUserAccessException);

            var expectedActionResult =
                new ActionResult<UserAccess>(expectedNotFoundObjectResult);

            this.userAccessServiceMock.Setup(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(userAccessValidationException);

            // when
            ActionResult<UserAccess> actualActionResult =
                await this.userAccessesController.DeleteUserAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.userAccessServiceMock.Verify(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfRecordIsLockedAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var lockedUserAccessException =
                new LockedUserAccessException(
                    message: someMessage,
                    innerException: someInnerException);

            var userAccessDependencyValidationException =
                new UserAccessDependencyValidationException(
                    message: someMessage,
                    innerException: lockedUserAccessException);

            LockedObjectResult expectedConflictObjectResult =
                Locked(lockedUserAccessException);

            var expectedActionResult =
                new ActionResult<UserAccess>(expectedConflictObjectResult);

            this.userAccessServiceMock.Setup(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(userAccessDependencyValidationException);

            // when
            ActionResult<UserAccess> actualActionResult =
                await this.userAccessesController.DeleteUserAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.userAccessServiceMock.Verify(service =>
                service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.userAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
