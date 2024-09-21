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
        public async Task GetLookupByIdsAsyncShouldReturnLookup()
        {
            // given
            Lookup randomLookup = CreateRandomLookup();
            Guid inputId = randomLookup.Id;
            Lookup storageLookup = randomLookup;
            Lookup expectedLookup = storageLookup.DeepClone();

            lookupServiceMock
                .Setup(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageLookup);

            // when
            var result = await lookupsController.GetLookupByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedLookup);

            lookupServiceMock
                .Verify(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetLookupByIdsAsyncShouldReturnNotFoundWhenLookupValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var notFoundLookupException = new NotFoundLookupException(message: $"Lookup not found with Id: {someId}");

            var lookupValidationException = new LookupValidationException(
                message: GetRandomString(),
                innerException: notFoundLookupException);

            lookupServiceMock
                .Setup(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await lookupsController.GetLookupByIdAsync(someId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);

            lookupServiceMock
                .Verify(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetLookupByIdsAsyncShouldReturnBadRequestWhenLookupValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new LookupValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            lookupServiceMock
                .Setup(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await lookupsController.GetLookupByIdAsync(someId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            lookupServiceMock
               .Verify(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetLookupByIdsAsyncShouldReturnInternalServerErrorWhenLookupDependencyExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new LookupDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            lookupServiceMock
                .Setup(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await lookupsController.GetLookupByIdAsync(someId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            lookupServiceMock
               .Verify(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetLookupByIdsAsyncShouldReturnInternalServerErrorWhenLookupServiceExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new LookupServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            lookupServiceMock
                .Setup(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await lookupsController.GetLookupByIdAsync(someId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            lookupServiceMock
               .Verify(service => service.RetrieveLookupByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            lookupServiceMock.VerifyNoOtherCalls();
        }
    }
}
