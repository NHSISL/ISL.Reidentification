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
        public async Task GetOdsDataByIdAsyncShouldReturnOdsData()
        {
            // given
            OdsData randomOdsData = CreateRandomOdsData();
            Guid inputOdsDataId = randomOdsData.Id;
            OdsData storageOdsData = randomOdsData.DeepClone();
            OdsData expectedOdsData = storageOdsData.DeepClone();

            this.odsDataServiceMock.Setup(service =>
                service.RetrieveOdsDataByIdAsync(inputOdsDataId))
                    .ReturnsAsync(storageOdsData);

            // when
            var result = await this.odsDataController.GetOdsDataByIdAsync(inputOdsDataId);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedOdsData);
        }

        [Fact]
        public async Task GetOdsDataByIdAsyncShouldReturnNotFoundWhenOdsDataValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var notFoundOdsDataException = new NotFoundOdsDataException(odsDataId: inputId);

            var odsDataValidationException = new OdsDataValidationException(
                message: GetRandomString(),
                innerException: notFoundOdsDataException);

            this.odsDataServiceMock
                .Setup(service => service.RetrieveOdsDataByIdAsync(inputId))
                .ThrowsAsync(odsDataValidationException);

            // when
            var result = await this.odsDataController.GetOdsDataByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetOdsDataByIdAsyncShouldReturnBadRequestWhenOdsDataValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOdsDataId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var odsDataValidationException = new OdsDataValidationException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.odsDataServiceMock.Setup(service =>
                service.RetrieveOdsDataByIdAsync(inputOdsDataId))
                    .ThrowsAsync(odsDataValidationException);

            // when
            var result = await this.odsDataController.GetOdsDataByIdAsync(inputOdsDataId);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetOdsDataByIdAsyncShouldReturnInternalServerErrorWhenOdsDataDependencyExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputOdsDataId = randomId;
            Xeption randomXeption = new Xeption(message: GetRandomString());

            var odsDataDependencyException = new OdsDataDependencyException(
                message: GetRandomString(),
                innerException: randomXeption);

            this.odsDataServiceMock.Setup(service =>
                service.RetrieveOdsDataByIdAsync(inputOdsDataId))
                    .ThrowsAsync(odsDataDependencyException);

            // when
            var result = await this.odsDataController.GetOdsDataByIdAsync(inputOdsDataId);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
