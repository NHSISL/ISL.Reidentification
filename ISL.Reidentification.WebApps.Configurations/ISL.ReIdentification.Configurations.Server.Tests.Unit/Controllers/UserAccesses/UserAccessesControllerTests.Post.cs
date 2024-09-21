// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
        public async Task PostUserAccessAsyncShouldReturnCreatedWhenUserAccessIsAdded()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            UserAccess storageUserAccess = inputUserAccess.DeepClone();
            UserAccess expectedUserAccess = storageUserAccess.DeepClone();

            userAccessServiceMock
            .Setup(service => service.AddUserAccessAsync(inputUserAccess))
                    .ReturnsAsync(storageUserAccess);

            // when
            var result = await userAccessesController.PostUserAccessAsync(randomUserAccess);

            // then
            var createdResult = Assert.IsType<CreatedObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(expectedUserAccess);

            userAccessServiceMock
               .Verify(service => service.AddUserAccessAsync(inputUserAccess),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PostUserAccessAsyncShouldReturnBadRequestWhenUserAccessValidationExceptionOccurs()
        {
            // given
            UserAccess someUserAccess = CreateRandomUserAccess();
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await userAccessesController.PostUserAccessAsync(someUserAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            userAccessServiceMock
               .Verify(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PostUserAccessAsyncShouldReturnConflictWhenAlreadyExistsUserAccessExceptionOccurs()
        {
            // given
            UserAccess someUserAccess = CreateRandomUserAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var alreadyExistsException = new AlreadyExistsUserAccessException(
                message: GetRandomString(),
                innerException: someXeption,
                data: someXeption.Data);

            var dependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: alreadyExistsException);

            userAccessServiceMock
                .Setup(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await userAccessesController.PostUserAccessAsync(someUserAccess);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);

            userAccessServiceMock
               .Verify(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            PostUserAccessAsyncShouldReturnBadRequestWhenUserAccessDependencyValidationExceptionOccurs()
        {
            // given
            UserAccess someUserAccess = CreateRandomUserAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            userAccessServiceMock
                .Setup(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await userAccessesController.PostUserAccessAsync(someUserAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            userAccessServiceMock
               .Verify(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            PostUserAccessAsyncShouldReturnInternalServerErrorWhenUserAccessDependencyExceptionOccurs()
        {
            // given
            UserAccess someUserAccess = CreateRandomUserAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new UserAccessDependencyException(
                message: GetRandomString(),
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await userAccessesController.PostUserAccessAsync(someUserAccess);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            userAccessServiceMock
               .Verify(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            PostUserAccessAsyncShouldReturnInternalServerErrorWhenUserAccessServiceExceptionOccurs()
        {
            // given
            UserAccess someUserAccess = CreateRandomUserAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new UserAccessServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await userAccessesController.PostUserAccessAsync(someUserAccess);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            userAccessServiceMock
               .Verify(service => service.AddUserAccessAsync(It.IsAny<UserAccess>()),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
