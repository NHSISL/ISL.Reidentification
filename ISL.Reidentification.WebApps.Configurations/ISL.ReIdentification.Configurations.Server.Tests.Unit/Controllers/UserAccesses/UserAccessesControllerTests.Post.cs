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

            mockUserAccessService.Setup(service =>
                service.AddUserAccessAsync(inputUserAccess))
                    .ReturnsAsync(storageUserAccess);

            // when
            var result = await userAccessesController.PostUserAccessAsync(randomUserAccess);

            // then
            var createdResult = Assert.IsType<CreatedObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(expectedUserAccess);
        }

        [Fact]
        public async Task PostUserAccessAsyncShouldReturnBadRequestWhenUserAccessValidationExceptionOccurs()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: randomXeption);

            mockUserAccessService.Setup(service =>
                service.AddUserAccessAsync(inputUserAccess))
                    .ThrowsAsync(userAccessValidationException);

            // when
            var result = await userAccessesController.PostUserAccessAsync(inputUserAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PostUserAccessAsyncShouldReturnConflictWhenAlreadyExistsUserAccessExceptionOccurs()
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

            mockUserAccessService.Setup(service =>
                service.AddUserAccessAsync(inputUserAccess))
                    .ThrowsAsync(userAccessDependencyValidationException);

            // when
            var result = await userAccessesController.PostUserAccessAsync(inputUserAccess);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);
        }
    }
}
