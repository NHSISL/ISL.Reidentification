// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using Microsoft.AspNetCore.Mvc;
using Moq;

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

            this.odsServiceMock.Setup(service =>
                service.RetrieveAllOdsDatasAsync())
                    .ReturnsAsync(storageOdsDatas);

            // when
            var result = await this.odsDataController.GetAsync();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedOdsDatas);
        }
    }
}
