// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task PutDelegatedAccessAsyncShouldReturnOkWhenDelegatedAccessIsUpdated()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess storageDelegatedAccess = inputDelegatedAccess.DeepClone();
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAccess.DeepClone();

            delegatedAccessServiceMock
            .Setup(service => service.ModifyDelegatedAccessAsync(inputDelegatedAccess))
                    .ReturnsAsync(storageDelegatedAccess);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(randomDelegatedAccess);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);

            delegatedAccessServiceMock
               .Verify(service => service.ModifyDelegatedAccessAsync(inputDelegatedAccess),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PutDelegatedAccessAsyncShouldReturnBadRequestWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(someDelegatedAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            delegatedAccessServiceMock
               .Verify(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PutDelegatedAccessAsyncShouldReturnConflictWhenAlreadyExistsDelegatedAccessExceptionOccurs()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var alreadyExistsException = new AlreadyExistsDelegatedAccessException(
                message: GetRandomString(),
                innerException: someXeption,
                data: someXeption.Data);

            var dependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: alreadyExistsException);

            delegatedAccessServiceMock
                .Setup(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(someDelegatedAccess);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);

            delegatedAccessServiceMock
               .Verify(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            PutDelegatedAccessAsyncShouldReturnBadRequestWhenDelegatedAccessDependencyValidationExceptionOccurs()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(someDelegatedAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            delegatedAccessServiceMock
               .Verify(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            PutDelegatedAccessAsyncShouldReturnInternalServerErrorWhenDelegatedAccessDependencyExceptionOccurs()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new DelegatedAccessDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(someDelegatedAccess);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            delegatedAccessServiceMock
               .Verify(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            PutDelegatedAccessAsyncShouldReturnInternalServerErrorWhenDelegatedAccessServiceExceptionOccurs()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new DelegatedAccessServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(someDelegatedAccess);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            delegatedAccessServiceMock
               .Verify(service => service.ModifyDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
