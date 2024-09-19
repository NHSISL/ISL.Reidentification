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
        public async Task GetAllPdsDataAsyncShouldReturnItems()
        {
            // given
            IQueryable<PdsData> randomPdsData = CreateRandomPdsDatas();
            IQueryable<PdsData> storagePdsData = randomPdsData.DeepClone();
            IQueryable<PdsData> expectedPdsData = storagePdsData.DeepClone();

            this.mockPdsDataService
                .Setup(service => service.RetrieveAllPdsDataAsync())
                .ReturnsAsync(storagePdsData);

            // when
            var result = await this.pdsDataController.Get();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedPdsData);
        }

        [Fact]
        public async Task GetAllPdsDataAsyncShouldReturnInternalServerErrorWhenPdsDataDependencyExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var dependencyException = new PdsDataDependencyException(
                message: GetRandomString(),
                innerException: someXeption);

            mockPdsDataService
                .Setup(service => service.RetrieveAllPdsDataAsync())
                .ThrowsAsync(dependencyException);

            // when
            var result = await pdsDataController.Get();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAllPdsDataAsyncShouldReturnInternalServerErrorWhenPdsDataServiceExceptionOccurs()
        {
            // given
            var someXeption = new Xeption(message: GetRandomString());

            var serviceException = new PdsDataServiceException(
                message: GetRandomString(),
                innerException: someXeption);

            mockPdsDataService
                .Setup(service => service.RetrieveAllPdsDataAsync())
                .ThrowsAsync(serviceException);

            // when
            var result = await pdsDataController.Get();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
