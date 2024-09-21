// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task GetAllPdsDatasAsyncShouldReturnItems()
        {
            // given
            IQueryable<PdsData> randomPdsDatas = CreateRandomPdsDatas();
            IQueryable<PdsData> storagePdsDatas = randomPdsDatas.DeepClone();
            IQueryable<PdsData> expectedPdsData = storagePdsDatas.DeepClone();
            pdsDataServiceMock
                .Setup(service => service.RetrieveAllPdsDatasAsync())
                    .ReturnsAsync(storagePdsDatas);

            // when
            var result = await pdsDataController.GetAsync();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedPdsData);

            pdsDataServiceMock
               .Verify(service => service.RetrieveAllPdsDatasAsync(),
                   Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllPdsDatasAsyncShouldReturnInternalServerErrorWhenPdsDataDependencyExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new PdsDataDependencyException(
                message: GetRandomString(),
                innerException: someXeption);
            pdsDataServiceMock
                .Setup(service => service.RetrieveAllPdsDatasAsync())
                    .ThrowsAsync(dependencyException);

            // when
            var result = await pdsDataController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            pdsDataServiceMock
               .Verify(service => service.RetrieveAllPdsDatasAsync(),
                   Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAllPdsDatasAsyncShouldReturnInternalServerErrorWhenPdsDataServiceExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var lookupServiceException = new PdsDataServiceException(
                message: "Service error occurred, contact support.",
                innerException: someXeption);
            pdsDataServiceMock
                .Setup(service => service.RetrieveAllPdsDatasAsync())
                    .ThrowsAsync(lookupServiceException);

            // when
            var result = await pdsDataController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);

            pdsDataServiceMock
               .Verify(service => service.RetrieveAllPdsDatasAsync(),
                   Times.Once);

            pdsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
