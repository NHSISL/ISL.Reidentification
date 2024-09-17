// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task GetAllUserAccessesAsyncShouldReturnItems()
        {
            // given
            IQueryable<UserAccess> randomUserAccesses = CreateRandomUserAccesses();
            IQueryable<UserAccess> storageUserAccesses = randomUserAccesses.DeepClone();
            IQueryable<UserAccess> expectedUserAccesses = storageUserAccesses.DeepClone();

            this.mockUserAccessService.Setup(service =>
                service.RetrieveAllUserAccessesAsync())
                    .ReturnsAsync(storageUserAccesses);

            // when
            var result = await this.userAccessesController.Get();

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedUserAccesses);
        }

        [Fact]
        public async Task GetAllUserAccesssAsyncShouldReturnInternalServerErrorWhenUserAccessDependencyExceptionOccurs()
        {
            // given
            var randomXeption = new Xeption(message: GetRandomString());

            var userAccessDependencyException = new UserAccessDependencyException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.RetrieveAllUserAccessesAsync())
                    .ThrowsAsync(userAccessDependencyException);

            // when
            var result = await this.userAccessesController.Get();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAllUserAccesssAsyncShouldReturnInternalServerErrorWhenUserAccessServiceExceptionOccurs()
        {
            // given
            var randomXeption = new Xeption(message: GetRandomString());

            var userAccessServiceException = new UserAccessServiceException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.mockUserAccessService.Setup(service =>
                service.RetrieveAllUserAccessesAsync())
                    .ThrowsAsync(userAccessServiceException);

            // when
            var result = await this.userAccessesController.Get();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
