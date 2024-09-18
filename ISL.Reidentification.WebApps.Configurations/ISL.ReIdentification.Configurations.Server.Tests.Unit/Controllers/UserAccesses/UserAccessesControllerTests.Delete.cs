// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.UserAccesses
{
    public partial class UserAccessesControllerTests
    {
        [Fact]
        public async Task DeleteUserAccessByIdAsyncShouldReturnUserAccess()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            Guid inputUserAccessId = randomUserAccess.Id;
            UserAccess storageUserAccess = randomUserAccess.DeepClone();
            UserAccess expectedUserAccess = storageUserAccess.DeepClone();

            this.mockUserAccessService.Setup(service =>
                service.RemoveUserAccessByIdAsync(inputUserAccessId))
                    .ReturnsAsync(storageUserAccess);

            // when
            var result = await this.userAccessesController.DeleteUserAccessByIdAsync(inputUserAccessId);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedUserAccess);
        }

        [Fact]
        public async Task DeleteUserAccessByIdAsyncShouldReturnNotFoundWhenUserAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var notFoundUserAccessException = new NotFoundUserAccessException(message: GetRandomString());

            var userAccessValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: notFoundUserAccessException);

            this.mockUserAccessService
                .Setup(service => service.RemoveUserAccessByIdAsync(inputId))
                .ThrowsAsync(userAccessValidationException);

            // when
            var result = await this.userAccessesController.DeleteUserAccessByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteUserAccessByIdAsyncShouldReturnBadRequestWhenUserAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputUserAccessId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.RemoveUserAccessByIdAsync(inputUserAccessId))
                    .ThrowsAsync(userAccessValidationException);

            // when
            var result = await this.userAccessesController.DeleteUserAccessByIdAsync(inputUserAccessId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task
            DeleteUserAccessByIdAsyncShouldReturnLockedErrorWhenUserAccessDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputUserAccessId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var lockedUserAccessException =
                new LockedUserAccessException(message: GetRandomString(), innerException: randomXeption);

            var userAccessDependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: lockedUserAccessException);

            this.mockUserAccessService.Setup(service =>
                service.RemoveUserAccessByIdAsync(inputUserAccessId))
                    .ThrowsAsync(userAccessDependencyValidationException);

            // when
            var result = await this.userAccessesController.DeleteUserAccessByIdAsync(inputUserAccessId);

            // then
            var lockedObjectResult = Assert.IsType<LockedObjectResult>(result.Result);
            lockedObjectResult.StatusCode.Should().Be(423);
        }

        [Fact]
        public async Task DeleteUserAccessAsyncShouldReturnBadRequestWhenUserAccessDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputUserAccessId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessDependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.RemoveUserAccessByIdAsync(inputUserAccessId))
                    .ThrowsAsync(userAccessDependencyValidationException);

            // when
            var result = await this.userAccessesController.DeleteUserAccessByIdAsync(inputUserAccessId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task
            DeleteUserAccessByIdAsyncShouldReturnInternalServerErrorWhenUserAccessDependencyExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputUserAccessId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessDependencyException = new UserAccessDependencyException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.RemoveUserAccessByIdAsync(inputUserAccessId))
                    .ThrowsAsync(userAccessDependencyException);

            // when
            var result = await this.userAccessesController.DeleteUserAccessByIdAsync(inputUserAccessId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DeleteUserAccessByIdAsyncShouldReturnInternalServerErrorWhenUserAccessServiceExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputUserAccessId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessServiceException = new UserAccessServiceException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.RemoveUserAccessByIdAsync(inputUserAccessId))
                    .ThrowsAsync(userAccessServiceException);

            // when
            var result = await this.userAccessesController.DeleteUserAccessByIdAsync(inputUserAccessId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
