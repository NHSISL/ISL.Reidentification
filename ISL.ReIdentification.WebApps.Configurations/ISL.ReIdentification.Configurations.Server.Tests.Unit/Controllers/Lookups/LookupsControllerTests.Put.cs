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
        public async Task PutLookupAsyncShouldReturnOkWhenLookupIsUpdated()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Lookup inputLookup = randomLookup;
            Lookup storageLookup = inputLookup.DeepClone();
            Lookup expectedLookup = storageLookup.DeepClone();

            lookupServiceMock
                .Setup(service => service.ModifyLookupAsync(inputLookup))
                    .ReturnsAsync(storageLookup);

            // when
            var result = await lookupsController.PutLookupAsync(randomLookup);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedLookup);

            lookupServiceMock
               .Verify(service => service.ModifyLookupAsync(inputLookup),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PutLookupAsyncShouldReturnBadRequestWhenLookupValidationExceptionOccurs()
        {
            // given
            Lookup someLookup = CreateRandomLookup();
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new LookupValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            lookupServiceMock
                .Setup(service => service.ModifyLookupAsync(It.IsAny<Lookup>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await lookupsController.PutLookupAsync(someLookup);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            lookupServiceMock
               .Verify(service => service.ModifyLookupAsync(It.IsAny<Lookup>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PutLookupAsyncShouldReturnConflictWhenAlreadyExistsLookupExceptionOccurs()
        {
            // given
            Lookup someLookup = CreateRandomLookup();
            var someXeption = new Xeption(message: GetRandomString());

            var alreadyExistsException = new AlreadyExistsLookupException(
                message: GetRandomString(),
                innerException: someXeption,
                data: someXeption.Data);

            var dependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: alreadyExistsException);

            lookupServiceMock
                .Setup(service => service.ModifyLookupAsync(It.IsAny<Lookup>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await lookupsController.PutLookupAsync(someLookup);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);

            lookupServiceMock
               .Verify(service => service.ModifyLookupAsync(It.IsAny<Lookup>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PutLookupAsyncShouldReturnBadRequestWhenLookupDependencyValidationExceptionOccurs()
        {
            // given
            Lookup someLookup = CreateRandomLookup();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            lookupServiceMock
                .Setup(service => service.ModifyLookupAsync(It.IsAny<Lookup>()))
                    .ThrowsAsync(dependencyValidationException);

            // when
            var result = await lookupsController.PutLookupAsync(someLookup);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            lookupServiceMock
               .Verify(service => service.ModifyLookupAsync(It.IsAny<Lookup>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PutLookupAsyncShouldReturnInternalServerErrorWhenLookupDependencyExceptionOccurs()
        {
            // given
            Lookup someLookup = CreateRandomLookup();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new LookupDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            lookupServiceMock
                .Setup(service => service.ModifyLookupAsync(It.IsAny<Lookup>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await lookupsController.PutLookupAsync(someLookup);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            lookupServiceMock
               .Verify(service => service.ModifyLookupAsync(It.IsAny<Lookup>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PutLookupAsyncShouldReturnInternalServerErrorWhenLookupServiceExceptionOccurs()
        {
            // given
            Lookup someLookup = CreateRandomLookup();
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new LookupServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            lookupServiceMock
                .Setup(service => service.ModifyLookupAsync(It.IsAny<Lookup>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await lookupsController.PutLookupAsync(someLookup);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            lookupServiceMock
               .Verify(service => service.ModifyLookupAsync(It.IsAny<Lookup>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }
    }
}
