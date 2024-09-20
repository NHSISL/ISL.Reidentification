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
    }
}
