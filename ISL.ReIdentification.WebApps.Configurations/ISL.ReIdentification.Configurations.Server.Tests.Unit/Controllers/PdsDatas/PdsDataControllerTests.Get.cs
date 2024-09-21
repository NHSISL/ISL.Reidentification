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
        public async Task GetPdsDataByIdsAsyncShouldReturnPdsData()
        {
            // given
            PdsData randomPdsData = CreateRandomPdsData();
            Guid inputId = randomPdsData.Id;
            PdsData storagePdsData = randomPdsData;
            PdsData expectedPdsData = storagePdsData.DeepClone();
            pdsDataServiceMock
                .Setup(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storagePdsData);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedPdsData);

            pdsDataServiceMock
                .Verify(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnNotFoundWhenPdsDataValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var notFoundPdsDataException = new NotFoundPdsDataException(
                message: $"PDS data not found with Id: {someId}");

            var lookupValidationException = new PdsDataValidationException(
                message: GetRandomString(),
                innerException: notFoundPdsDataException);
            pdsDataServiceMock
                .Setup(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(someId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);

            pdsDataServiceMock
                .Verify(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnBadRequestWhenPdsDataValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new PdsDataValidationException(
                message: GetRandomString(),
                innerException: someXeption);
            pdsDataServiceMock
                .Setup(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(someId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            pdsDataServiceMock
               .Verify(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnInternalServerErrorWhenPdsDataDependencyExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new PdsDataDependencyException(
                message: GetRandomString(),
                innerException: someXeption);
            pdsDataServiceMock
                .Setup(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(someId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            pdsDataServiceMock
               .Verify(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetPdsDataByIdsAsyncShouldReturnInternalServerErrorWhenPdsDataServiceExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new PdsDataServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);
            pdsDataServiceMock
                .Setup(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await pdsDataController.GetPdsDataByIdAsync(someId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            pdsDataServiceMock
               .Verify(service => service.RetrievePdsDataByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
