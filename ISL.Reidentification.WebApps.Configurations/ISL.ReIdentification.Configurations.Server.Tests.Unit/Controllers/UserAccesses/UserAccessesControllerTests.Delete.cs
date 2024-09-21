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
        public async Task DeleteUserAccessByIdsAsyncShouldReturnUserAccess()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            Guid inputId = randomUserAccess.Id;
            UserAccess storageUserAccess = randomUserAccess;
            UserAccess expectedUserAccess = storageUserAccess.DeepClone();

            userAccessServiceMock
            .Setup(service => service.RemoveUserAccessByIdAsync(inputId))
                    .ReturnsAsync(storageUserAccess);

            // when
            var result = await userAccessesController.DeleteUserAccessByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedUserAccess);

            userAccessServiceMock
                .Verify(service => service.RemoveUserAccessByIdAsync(inputId),
                    Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteUserAccessByIdsAsyncShouldReturnNotFoundWhenUserAccessValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var notFoundUserAccessException = new NotFoundUserAccessException(
                message: $"Access audit not found with Id: {someId}");

            var lookupValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: notFoundUserAccessException);
            userAccessServiceMock
                .Setup(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await userAccessesController.DeleteUserAccessByIdAsync(someId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);

            userAccessServiceMock
                .Verify(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteUserAccessByIdsAsyncShouldReturnBadRequestWhenUserAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await userAccessesController.DeleteUserAccessByIdAsync(inputId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            userAccessServiceMock
                .Verify(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteUserAccessByIdsAsyncShouldReturnLockedErrorWhenUserAccessDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var lockedUserAccessException =
                new LockedUserAccessException(message: GetRandomString(), innerException: someXeption);

            var lookupDependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: lockedUserAccessException);

            userAccessServiceMock
                .Setup(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupDependencyValidationException);

            // when
            var result = await userAccessesController.DeleteUserAccessByIdAsync(inputId);

            // then
            var lockedObjectResult = Assert.IsType<LockedObjectResult>(result.Result);
            lockedObjectResult.StatusCode.Should().Be(423);

            userAccessServiceMock
                .Verify(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteUserAccessAsyncShouldReturnBadRequestWhenUserAccessDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await userAccessesController.DeleteUserAccessByIdAsync(inputId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            userAccessServiceMock
                .Verify(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteUserAccessByIdsAsyncShouldReturnInternalServerErrorWhenUserAccessDependencyExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new UserAccessDependencyException(
                message: GetRandomString(),
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await userAccessesController.DeleteUserAccessByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            userAccessServiceMock
                .Verify(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteUserAccessByIdsAsyncShouldReturnInternalServerErrorWhenUserAccessServiceExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new UserAccessServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await userAccessesController.DeleteUserAccessByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            userAccessServiceMock
                .Verify(service => service.RemoveUserAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
