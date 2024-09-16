// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.Lookups
{
    public partial class LookupsControllerTests
    {
        [Fact]
        public async Task DeleteLookupByIdsAsyncShouldReturnLookup()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Guid inputId = randomLookup.Id;
            Lookup storageLookup = randomLookup;
            Lookup expectedLookup = storageLookup.DeepClone();

            mockLookupService
                .Setup(service => service.RemoveLookupByIdAsync(inputId))
                .ReturnsAsync(storageLookup);

            // when
            var result = await lookupsController.DeleteLookupByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedLookup);
        }

        [Fact]
        public async Task DeleteLookupByIdsAsyncShouldReturnNotFoundWhenLookupValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var notFoundLookupException = new NotFoundLookupException(lookupId: inputId);

            var lookupValidationException = new LookupValidationException(
                message: GetRandomString(),
                innerException: notFoundLookupException);

            mockLookupService
                .Setup(service => service.RemoveLookupByIdAsync(inputId))
                .ThrowsAsync(lookupValidationException);

            // when
            var result = await lookupsController.DeleteLookupByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeleteLookupByIdsAsyncShouldReturnBadRequestWhenLookupValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new LookupValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.RemoveLookupByIdAsync(inputId))
                .ThrowsAsync(lookupValidationException);

            // when
            var result = await lookupsController.DeleteLookupByIdAsync(inputId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task DeleteLookupByIdsAsyncShouldReturnLockedErrorWhenLookupDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var lockedLookupException =
                new LockedLookupException(message: GetRandomString(), innerException: someXeption);

            var lookupDependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: lockedLookupException);

            mockLookupService
                .Setup(service => service.RemoveLookupByIdAsync(inputId))
                .ThrowsAsync(lookupDependencyValidationException);

            // when
            var result = await lookupsController.DeleteLookupByIdAsync(inputId);

            // then
            var lockedObjectResult = Assert.IsType<LockedObjectResult>(result.Result);
            lockedObjectResult.StatusCode.Should().Be(423);
        }

        [Fact]
        public async Task DeleteLookupAsyncShouldReturnBadRequestWhenLookupDependencyValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.RemoveLookupByIdAsync(inputId))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await lookupsController.DeleteLookupByIdAsync(inputId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task DeleteLookupByIdsAsyncShouldReturnInternalServerErrorWhenLookupDependencyExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new LookupDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.RemoveLookupByIdAsync(inputId))
                .ThrowsAsync(dependencyException);

            // when
            var result = await lookupsController.DeleteLookupByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task DeleteLookupByIdsAsyncShouldReturnInternalServerErrorWhenLookupServiceExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new LookupServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.RemoveLookupByIdAsync(inputId))
                .ThrowsAsync(lookupServiceException);

            // when
            var result = await lookupsController.DeleteLookupByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
