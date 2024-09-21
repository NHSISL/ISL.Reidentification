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
            IQueryable<UserAccess> expectedUserAccess = storageUserAccesses.DeepClone();
            userAccessServiceMock
                .Setup(service => service.RetrieveAllUserAccessesAsync())
                    .ReturnsAsync(storageUserAccesses);

            // when
            var result = await userAccessesController.GetAsync();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedUserAccess);

            userAccessServiceMock
               .Verify(service => service.RetrieveAllUserAccessesAsync(),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            GetAllUserAccessesAsyncShouldReturnInternalServerErrorWhenUserAccessDependencyExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new UserAccessDependencyException(
                message: GetRandomString(),
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.RetrieveAllUserAccessesAsync())
                    .ThrowsAsync(dependencyException);

            // when
            var result = await userAccessesController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            userAccessServiceMock
               .Verify(service => service.RetrieveAllUserAccessesAsync(),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            GetAllUserAccessesAsyncShouldReturnInternalServerErrorWhenUserAccessServiceExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new UserAccessServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);
            userAccessServiceMock
                .Setup(service => service.RetrieveAllUserAccessesAsync())
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await userAccessesController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            userAccessServiceMock
               .Verify(service => service.RetrieveAllUserAccessesAsync(),
                   Times.Once);

            userAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
