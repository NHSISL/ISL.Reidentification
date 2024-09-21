// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task DeleteDelegatedAccessByIdsAsyncShouldReturnDelegatedAccess()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            Guid inputId = randomDelegatedAccess.Id;
            DelegatedAccess storageDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAccess.DeepClone();

            delegatedAccessServiceMock
            .Setup(service => service.RemoveDelegatedAccessByIdAsync(inputId))
                    .ReturnsAsync(storageDelegatedAccess);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);

            delegatedAccessServiceMock
                .Verify(service => service.RemoveDelegatedAccessByIdAsync(inputId),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessByIdsAsyncShouldReturnNotFoundWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var notFoundDelegatedAccessException = new NotFoundDelegatedAccessException(
                message: $"Couldn't find PDS data with Id: {someId}.");

            var lookupValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: notFoundDelegatedAccessException);

            delegatedAccessServiceMock
                .Setup(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(someId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);

            delegatedAccessServiceMock
                .Verify(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessByIdsAsyncShouldReturnBadRequestWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            delegatedAccessServiceMock
                .Verify(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessByIdsAsyncShouldReturnLockedErrorWhenDelegatedAccessDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var lockedDelegatedAccessException =
                new LockedDelegatedAccessException(message: GetRandomString(), innerException: someXeption);

            var lookupDependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: lockedDelegatedAccessException);

            delegatedAccessServiceMock
                .Setup(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupDependencyValidationException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var lockedObjectResult = Assert.IsType<LockedObjectResult>(result.Result);
            lockedObjectResult.StatusCode.Should().Be(423);

            delegatedAccessServiceMock
                .Verify(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessAsyncShouldReturnBadRequestWhenDelegatedAccessDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            delegatedAccessServiceMock
                .Verify(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessByIdsAsyncShouldReturnInternalServerErrorWhenDelegatedAccessDependencyExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new DelegatedAccessDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            delegatedAccessServiceMock
                .Verify(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessByIdsAsyncShouldReturnInternalServerErrorWhenDelegatedAccessServiceExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new DelegatedAccessServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            delegatedAccessServiceMock
                .Setup(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            delegatedAccessServiceMock
                .Verify(service => service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
