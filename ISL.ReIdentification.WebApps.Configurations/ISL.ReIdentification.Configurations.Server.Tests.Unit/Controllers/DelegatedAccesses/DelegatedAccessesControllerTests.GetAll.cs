// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.DelegatedAccesses
{
    public partial class DelegatedAccessesControllerTests
    {
        [Fact]
        public async Task GetAllDelegatedAccessesAsyncShouldReturnItems()
        {
            // given
            IQueryable<DelegatedAccess> randomDelegatedAccesses = CreateRandomDelegatedAccesses();
            IQueryable<DelegatedAccess> storageDelegatedAccesses = randomDelegatedAccesses.DeepClone();
            IQueryable<DelegatedAccess> expectedDelegatedAccess = storageDelegatedAccesses.DeepClone();
            delegatedAccessServiceMock
                .Setup(service => service.RetrieveAllDelegatedAccessesAsync())
                    .ReturnsAsync(storageDelegatedAccesses);

            // when
            var result = await delegatedAccessesController.GetAsync();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);

            delegatedAccessServiceMock
               .Verify(service => service.RetrieveAllDelegatedAccessesAsync(),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            GetAllDelegatedAccessesAsyncShouldReturnInternalServerErrorWhenDelegatedAccessDependencyExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new DelegatedAccessDependencyException(
                message: GetRandomString(),
                innerException: someXeption);
            delegatedAccessServiceMock
                .Setup(service => service.RetrieveAllDelegatedAccessesAsync())
                    .ThrowsAsync(dependencyException);

            // when
            var result = await delegatedAccessesController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            delegatedAccessServiceMock
               .Verify(service => service.RetrieveAllDelegatedAccessesAsync(),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            GetAllDelegatedAccessesAsyncShouldReturnInternalServerErrorWhenDelegatedAccessServiceExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new DelegatedAccessServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);
            delegatedAccessServiceMock
                .Setup(service => service.RetrieveAllDelegatedAccessesAsync())
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await delegatedAccessesController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            delegatedAccessServiceMock
               .Verify(service => service.RetrieveAllDelegatedAccessesAsync(),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
