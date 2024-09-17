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
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.UserAccesses
{
    public partial class UserAccessesControllerTests
    {
        [Fact]
        public async Task GetUserAccessByIdAsyncShouldReturnUserAccess()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            Guid inputUserAccessId = randomUserAccess.Id;
            UserAccess storageUserAccess = randomUserAccess.DeepClone();
            UserAccess expectedUserAccess = storageUserAccess.DeepClone();

            this.mockUserAccessService.Setup(service =>
                service.RetrieveUserAccessByIdAsync(inputUserAccessId))
                    .ReturnsAsync(storageUserAccess);

            // when
            var result = await this.userAccessesController.GetUserAccessByIdAsync(inputUserAccessId);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedUserAccess);
        }

        [Fact]
        public async Task GetUserAccessByIdAsyncShouldReturnNotFoundWhenUserAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var notFoundUserAccessException = new NotFoundUserAccessException(message: GetRandomString());

            var userAccessValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: notFoundUserAccessException);

            this.mockUserAccessService
                .Setup(service => service.RetrieveUserAccessByIdAsync(inputId))
                .ThrowsAsync(userAccessValidationException);

            // when
            var result = await this.userAccessesController.GetUserAccessByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetUserAccessByIdAsyncShouldReturnBadRequestWhenUserAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputUserAccessId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var userAccessValidationException = new UserAccessValidationException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.RetrieveUserAccessByIdAsync(inputUserAccessId))
                    .ThrowsAsync(userAccessValidationException);

            // when
            var result = await this.userAccessesController.GetUserAccessByIdAsync(inputUserAccessId);

            // then
            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }
    }
}
