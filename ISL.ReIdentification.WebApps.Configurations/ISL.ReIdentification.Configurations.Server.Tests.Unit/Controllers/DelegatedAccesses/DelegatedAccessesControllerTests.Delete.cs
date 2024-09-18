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
            DelegatedAccess storageDelegatedAcccess = randomDelegatedAccess;
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAcccess.DeepClone();

            mockDelegatedAccessService.Setup(service =>
                service.RemoveDelegatedAccessByIdAsync(inputId))
                .ReturnsAsync(storageDelegatedAcccess);

            // when
            var result = await this.delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessByIdsAsyncShouldReturnNotFoundWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var notFoundDelegatedAccessException = new NotFoundDelegatedAccessException(message: inputId.ToString());

            var delegatedAccessValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: notFoundDelegatedAccessException);

            mockDelegatedAccessService
                .Setup(service => service.RemoveDelegatedAccessByIdAsync(inputId))
                .ThrowsAsync(delegatedAccessValidationException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task
            DeleteDelegatedAccessByIdsAsyncShouldReturnBadRequestWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            Xeption someXeption = new Xeption(message: GetRandomString());

            var delegatedAccessValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockDelegatedAccessService
            .Setup(service => service.RemoveDelegatedAccessByIdAsync(inputId))
                .ThrowsAsync(delegatedAccessValidationException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
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

            var delegatedAccessDependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: GetRandomString(),
                innerException: lockedDelegatedAccessException);

            mockDelegatedAccessService
            .Setup(service => service.RemoveDelegatedAccessByIdAsync(inputId))
                .ThrowsAsync(delegatedAccessDependencyValidationException);

            // when
            var result = await delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var lockedObjectResult = Assert.IsType<LockedObjectResult>(result.Result);
            lockedObjectResult.StatusCode.Should().Be(423);
        }
    }
}
