// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.PdsDatas
{
    public partial class PdsDataControllerTests
    {
        [Fact]
        public async Task GetPdsDataByIdAsyncShouldReturnItem()
        {
            // given
            PdsData randomPdsData = CreateRandomPdsData();
            Guid inputId = randomPdsData.Id;
            PdsData storagePdsData = randomPdsData;
            PdsData expectedPdsData = storagePdsData.DeepClone();

            mockPdsDataService
                .Setup(service => service.RetrievePdsDataByIdAsync(inputId))
                .ReturnsAsync(storagePdsData);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(inputId);

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedPdsData);
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnNotFoundWhenPdsDataValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var notFoundPdsDataException = new NotFoundPdsDataException(pdsDataId: inputId);

            var pdsDataValidationException = new PdsDataValidationException(
                message: GetRandomString(),
                innerException: notFoundPdsDataException);

            mockPdsDataService
                .Setup(service => service.RetrievePdsDataByIdAsync(inputId))
                .ThrowsAsync(pdsDataValidationException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnBadRequestWhenPdsDataValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var pdsDataValidationException = new PdsDataValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            mockPdsDataService
                .Setup(service => service.RetrievePdsDataByIdAsync(inputId))
                .ThrowsAsync(pdsDataValidationException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnInternalServerErrorWhenPdsDataDependencyExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new PdsDataDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            mockPdsDataService
                .Setup(service => service.RetrievePdsDataByIdAsync(inputId))
                .ThrowsAsync(dependencyException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnInternalServerErrorWhenPdsDataServiceExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var someXeption = new Xeption(message: GetRandomString());

            var pdsDataServiceException = new PdsDataServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            mockPdsDataService
                .Setup(service => service.RetrievePdsDataByIdAsync(inputId))
                .ThrowsAsync(pdsDataServiceException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(inputId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
