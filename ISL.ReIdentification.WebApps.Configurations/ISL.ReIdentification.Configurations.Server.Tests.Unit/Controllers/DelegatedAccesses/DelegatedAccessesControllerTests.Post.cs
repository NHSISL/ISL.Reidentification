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
        public async Task PostDelegatedAccessAsyncShouldReturnCreatedWhenDelegatedAccessIsAdded()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess storageDelegatedAccess = inputDelegatedAccess.DeepClone();
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAccess.DeepClone();

            mockDelegatedAccessService
                .Setup(service => service.AddDelegatedAccessAsync(inputDelegatedAccess))
                .ReturnsAsync(storageDelegatedAccess);

            // when
            var result = await delegatedAccessesController.PostDelegatedAccessAsync(randomDelegatedAccess);

            // then
            var createdResult = Assert.IsType<CreatedObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);
        }

        [Fact]
        public async Task PostDelegatedAccessAsyncShouldReturnBadRequestWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            Xeption someXeption = new Xeption(message: GetRandomString());

            var delegatedAccesValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockDelegatedAccessService
                .Setup(service => service.AddDelegatedAccessAsync(inputDelegatedAccess))
                .ThrowsAsync(delegatedAccesValidationException);

            // when
            var result = await delegatedAccessesController.PostDelegatedAccessAsync(inputDelegatedAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PostDelegatedAccessAsyncShouldReturnConflictWhenAlreadyExistsDelegatedAccessExceptionOccurs()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            var someXeption = new Xeption(message: GetRandomString());

            var alreadyExistsException = new AlreadyExistsDelegatedAccessException(
                message: GetRandomString(),
                innerException: someXeption,
                data: someXeption.Data);

            var dependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: alreadyExistsException);

            mockDelegatedAccessService
                .Setup(service => service.AddDelegatedAccessAsync(inputDelegatedAccess))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await delegatedAccessesController.PostDelegatedAccessAsync(inputDelegatedAccess);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);
        }

        [Fact]
        public async Task PostDelegatedAccessAsyncShouldReturnBadRequestWhenDelegatedAccessDependencyValidationExceptionOccurs()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockDelegatedAccessService
                .Setup(service => service.AddDelegatedAccessAsync(inputDelegatedAccess))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await delegatedAccessesController.PostDelegatedAccessAsync(inputDelegatedAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }
    }
}
