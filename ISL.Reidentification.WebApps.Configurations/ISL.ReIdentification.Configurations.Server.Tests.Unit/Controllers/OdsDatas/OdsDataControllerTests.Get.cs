// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.OdsDatas
{
    public partial class OdsDataControllerTests
    {
        [Fact]
        public async Task GetOdsDataByIdsAsyncShouldReturnOdsData()
        {
            // given
            OdsData randomOdsData = CreateRandomOdsData();
            Guid inputId = randomOdsData.Id;
            OdsData storageOdsData = randomOdsData;
            OdsData expectedOdsData = storageOdsData.DeepClone();

            odsDataServiceMock
                .Setup(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageOdsData);

            // when
            var result = await odsDataController.GetOdsDataByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedOdsData);

            odsDataServiceMock
                .Verify(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetOdsDataByIdsAsyncShouldReturnNotFoundWhenOdsDataValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var notFoundOdsDataException = new NotFoundOdsDataException(
                message: $"ODS data not found with Id: {someId}");

            var lookupValidationException = new OdsDataValidationException(
                message: GetRandomString(),
                innerException: notFoundOdsDataException);

            odsDataServiceMock
                .Setup(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await odsDataController.GetOdsDataByIdAsync(someId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);

            odsDataServiceMock
                .Verify(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetOdsDataByIdsAsyncShouldReturnBadRequestWhenOdsDataValidationExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            Xeption someXeption = new Xeption(message: GetRandomString());

            var lookupValidationException = new OdsDataValidationException(
                message: GetRandomString(),
                innerException: someXeption);

            odsDataServiceMock
                .Setup(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupValidationException);

            // when
            var result = await odsDataController.GetOdsDataByIdAsync(someId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);

            odsDataServiceMock
               .Verify(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetOdsDataByIdsAsyncShouldReturnInternalServerErrorWhenOdsDataDependencyExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new OdsDataDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            odsDataServiceMock
                .Setup(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(dependencyException);

            // when
            var result = await odsDataController.GetOdsDataByIdAsync(someId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            odsDataServiceMock
               .Verify(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetOdsDataByIdsAsyncShouldReturnInternalServerErrorWhenOdsDataServiceExceptionOccurs()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new OdsDataServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);

            odsDataServiceMock
                .Setup(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await odsDataController.GetOdsDataByIdAsync(someId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            odsDataServiceMock
               .Verify(service => service.RetrieveOdsDataByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
