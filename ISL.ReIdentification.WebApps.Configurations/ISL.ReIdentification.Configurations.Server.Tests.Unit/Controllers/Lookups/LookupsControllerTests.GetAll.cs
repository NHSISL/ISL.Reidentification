// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task GetAllLookupsAsyncShouldReturnItems()
        {
            // given
            IQueryable<Lookup> randomLookups = CreateRandomLookups();
            IQueryable<Lookup> storageLookups = randomLookups.DeepClone();
            IQueryable<Lookup> expectedLookup = storageLookups.DeepClone();

            mockLookupService
                .Setup(service => service.RetrieveAllLookupsAsync())
                .ReturnsAsync(storageLookups);

            // when
            var result = await lookupsController.Get();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedLookup);
        }

        [Fact]
        public async Task GetAllLookupsAsyncShouldReturnInternalServerErrorWhenLookupDependencyExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new LookupDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.RetrieveAllLookupsAsync())
                .ThrowsAsync(dependencyException);

            // when
            var result = await lookupsController.Get();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAllLookupsAsyncShouldReturnInternalServerErrorWhenLookupServiceExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new LookupServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            mockLookupService
                .Setup(service => service.RetrieveAllLookupsAsync())
                .ThrowsAsync(lookupServiceException);

            // when
            var result = await lookupsController.Get();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
