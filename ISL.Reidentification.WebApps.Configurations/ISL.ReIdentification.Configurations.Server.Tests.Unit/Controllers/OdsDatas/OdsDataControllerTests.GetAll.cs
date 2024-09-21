// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task GetAllOdsDatasAsyncShouldReturnItems()
        {
            // given
            IQueryable<OdsData> randomOdsDatas = CreateRandomOdsDatas();
            IQueryable<OdsData> storageOdsDatas = randomOdsDatas.DeepClone();
            IQueryable<OdsData> expectedOdsData = storageOdsDatas.DeepClone();
            odsDataServiceMock
                .Setup(service => service.RetrieveAllOdsDatasAsync())
                    .ReturnsAsync(storageOdsDatas);

            // when
            var result = await odsDataController.GetAsync();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedOdsData);

            odsDataServiceMock
               .Verify(service => service.RetrieveAllOdsDatasAsync(),
                   Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllOdsDatasAsyncShouldReturnInternalServerErrorWhenOdsDataDependencyExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new OdsDataDependencyException(
                message: GetRandomString(),
                innerException: someXeption);
            odsDataServiceMock
                .Setup(service => service.RetrieveAllOdsDatasAsync())
                    .ThrowsAsync(dependencyException);

            // when
            var result = await odsDataController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            odsDataServiceMock
               .Verify(service => service.RetrieveAllOdsDatasAsync(),
                   Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllOdsDatasAsyncShouldReturnInternalServerErrorWhenOdsDataServiceExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new OdsDataServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);
            odsDataServiceMock
                .Setup(service => service.RetrieveAllOdsDatasAsync())
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await odsDataController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            odsDataServiceMock
               .Verify(service => service.RetrieveAllOdsDatasAsync(),
                   Times.Once);

            odsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
