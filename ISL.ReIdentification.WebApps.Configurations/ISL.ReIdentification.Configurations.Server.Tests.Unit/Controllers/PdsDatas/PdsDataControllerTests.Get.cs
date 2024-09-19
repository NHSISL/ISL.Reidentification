// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Microsoft.AspNetCore.Mvc;
using Moq;

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

            this.mockPdsDataService
                .Setup(service => service.RetrievePdsDataByIdAsync(inputId))
                .ReturnsAsync(storagePdsData);

            // when
            var result = await this.pdsDataController.GetPdsDataByIdAsync(inputId);

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedPdsData);
        }
    }
}
