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

            mockDelegatedAccessService
                .Setup(service => service.ModifyDelegatedAccessAsync(inputDelegatedAccess))
                .ReturnsAsync(storageDelegatedAccess);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(randomDelegatedAccess);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);
        }

        [Fact]
        public async Task PutDelegatedAccessAsyncShouldReturnNotFoundWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            Guid inputId = randomDelegatedAccess.Id;
            var notFoundDelegatedAccessException = new NotFoundDelegatedAccessException(message: inputId.ToString());

            var delegatedAccessValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: notFoundDelegatedAccessException);

            mockDelegatedAccessService
            .Setup(service => service.ModifyDelegatedAccessAsync(randomDelegatedAccess))
                .ThrowsAsync(delegatedAccessValidationException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(randomDelegatedAccess);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task PutDelegatedAccessAsyncShouldReturnBadRequestWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            Xeption someXeption = new Xeption(message: GetRandomString());

            var delegatedAccessValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockDelegatedAccessService
            .Setup(service => service.ModifyDelegatedAccessAsync(inputDelegatedAccess))
                .ThrowsAsync(delegatedAccessValidationException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(inputDelegatedAccess);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PutDelegatedAccessAsyncShouldReturnConflictWhenAlreadyExistsDelegatedAccessExceptionOccurs()
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
                .Setup(service => service.ModifyDelegatedAccessAsync(inputDelegatedAccess))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await delegatedAccessesController.PutDelegatedAccessAsync(inputDelegatedAccess);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);
        }
    }
}
