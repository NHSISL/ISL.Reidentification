// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task PostLookupAsyncShouldReturnCreatedWhenLookupIsAdded()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            Lookup storageLookup = inputLookup.DeepClone();
            Lookup expectedLookup = storageLookup.DeepClone();

            mockLookupService
                .Setup(service => service.AddLookupAsync(inputLookup))
                .ReturnsAsync(storageLookup);

            // when
            var result = await lookupsController.PostLookupAsync(randomLookup);

            // then
            var createdResult = Assert.IsType<CreatedObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().BeEquivalentTo(expectedLookup);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnBadRequestWhenLookupValidationExceptionOccurs()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new LookupValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.AddLookupAsync(inputLookup))
                .ThrowsAsync(lookupValidationException);

            // when
            var result = await lookupsController.PostLookupAsync(inputLookup);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnConflictWhenAlreadyExistsLookupExceptionOccurs()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            var someXeption = new Xeption(message: GetRandomString());

            var alreadyExistsException = new AlreadyExistsLookupException(
                message: GetRandomString(),
                innerException: someXeption,
                data: someXeption.Data);

            var dependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: alreadyExistsException);

            mockLookupService
                .Setup(service => service.AddLookupAsync(inputLookup))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await lookupsController.PostLookupAsync(inputLookup);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnBadRequestWhenLookupDependencyValidationExceptionOccurs()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.AddLookupAsync(inputLookup))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await lookupsController.PostLookupAsync(inputLookup);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnInternalServerErrorWhenLookupDependencyExceptionOccurs()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new LookupDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.AddLookupAsync(inputLookup))
                .ThrowsAsync(dependencyException);

            // when
            var result = await lookupsController.PostLookupAsync(inputLookup);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnInternalServerErrorWhenLookupServiceExceptionOccurs()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new LookupServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.AddLookupAsync(inputLookup))
                .ThrowsAsync(lookupServiceException);

            // when
            var result = await lookupsController.PostLookupAsync(inputLookup);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}