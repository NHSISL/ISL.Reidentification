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
        public async Task GetAllOdsDataAsyncShouldReturnOdsData()
        {
            // given
            IQueryable<OdsData> randomOdsDatas = CreateRandomOdsDatas();
            IQueryable<OdsData> storageOdsDatas = randomOdsDatas.DeepClone();
            IQueryable<OdsData> expectedOdsDatas = storageOdsDatas.DeepClone();

            this.odsDataServiceMock.Setup(service =>
                service.RetrieveAllOdsDatasAsync())
                    .ReturnsAsync(storageOdsDatas);

            // when
            var result = await this.odsDataController.GetAsync();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedOdsDatas);
        }

        [Fact]
        public async Task GetAllOdsDataAsyncShouldReturnInternalServerErrorWhenOdsDataDependencyExceptionOccurs()
        {
            // given
            Xeption randomException = new Xeption(GetRandomString());

            var dependencyException = new OdsDataDependencyException(
                message: GetRandomString(),
                innerException: randomException);

            this.odsDataServiceMock.Setup(service =>
                service.RetrieveAllOdsDatasAsync())
                    .ThrowsAsync(dependencyException);

            // when
            var result = await this.odsDataController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }

        [Fact]
        public async Task GetAllOdsDataAsyncShouldReturnInternalServerErrorWhenOdsDataServiceExceptionOccurs()
        {
            // given
            Xeption randomException = new Xeption(GetRandomString());

            var serviceException = new OdsDataServiceException(
                message: GetRandomString(),
                innerException: randomException);

            this.odsDataServiceMock.Setup(service =>
                service.RetrieveAllOdsDatasAsync())
                    .ThrowsAsync(serviceException);

            // when
            var result = await this.odsDataController.GetAsync();

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            internalServerErrorResult.StatusCode.Should().Be(500);
        }
    }
}
