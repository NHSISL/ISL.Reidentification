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
        public async Task PutUserAccessAsyncShouldReturnOkWhenUserAccessIsUpdated()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            UserAccess storageUserAccess = inputUserAccess.DeepClone();
            UserAccess expectedUserAccess = storageUserAccess.DeepClone();

            this.mockUserAccessService.Setup(service =>
                service.ModifyUserAccessAsync(inputUserAccess))
                    .ReturnsAsync(storageUserAccess);

            // when
            var result = await this.userAccessesController.PutUserAccessAsync(randomUserAccess);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedUserAccess);
        }

        [Fact]
        public async Task PutUserAccessAsyncShouldReturnBadRequestWhenUserAccessValidationExceptionOccurs()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.ModifyUserAccessAsync(inputUserAccess))
                    .ThrowsAsync(userAccessValidationException);

            // when
            var result = await this.userAccessesController.PutUserAccessAsync(inputUserAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PutUserAccessAsyncShouldReturnConflictWhenAlreadyExistsUserAccessExceptionOccurs()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var alreadyExistsUserAccessException = new AlreadyExistsUserAccessException(
                message: GetRandomString(),
                innerException: randomXeption,
                data: randomXeption.Data);

            var userAccessDependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: alreadyExistsUserAccessException);

            this.mockUserAccessService.Setup(service =>
                service.ModifyUserAccessAsync(inputUserAccess))
                    .ThrowsAsync(userAccessDependencyValidationException);

            // when
            var result = await this.userAccessesController.PutUserAccessAsync(inputUserAccess);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);
        }

        [Fact]
        public async Task PutUserAccessAsyncShouldReturnBadRequestWhenUserAccessDependencyValidationExceptionOccurs()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessDependencyValidationException = new UserAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.ModifyUserAccessAsync(inputUserAccess))
                    .ThrowsAsync(userAccessDependencyValidationException);

            // when
            var result = await this.userAccessesController.PutUserAccessAsync(inputUserAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PutUserAccessAsyncShouldReturnInternalServerErrorWhenUserAccessDependencyExceptionOccurs()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessDependencyException = new UserAccessDependencyException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.ModifyUserAccessAsync(inputUserAccess))
                    .ThrowsAsync(userAccessDependencyException);

            // when
            var result = await this.userAccessesController.PutUserAccessAsync(inputUserAccess);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task PutUserAccessAsyncShouldReturnInternalServerErrorWhenUserAccessServiceExceptionOccurs()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessServiceException = new UserAccessServiceException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.ModifyUserAccessAsync(inputUserAccess))
                    .ThrowsAsync(userAccessServiceException);

            // when
            var result = await this.userAccessesController.PutUserAccessAsync(inputUserAccess);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
