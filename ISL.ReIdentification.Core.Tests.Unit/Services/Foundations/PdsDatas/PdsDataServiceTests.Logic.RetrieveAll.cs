// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.PdsDatas
{
    public partial class PdsDataServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllPdsDataAsync()
        {
            //given
            IQueryable<PdsData> randomPdsData = CreateRandomPdsDatas();
            IQueryable<PdsData> storagePdsData = randomPdsData;
            IQueryable<PdsData> expectedPdsData = storagePdsData;

            this.odsStorageBroker.Setup(broker =>
                broker.SelectAllPdsDatasAsync())
                    .ReturnsAsync(storagePdsData);

            //when
            IQueryable<PdsData> actualPdsData =
                await this.pdsDataService.RetrieveAllPdsDataAsync();

            // then
            actualPdsData.Should().BeEquivalentTo(expectedPdsData);

            this.odsStorageBroker.Verify(broker =>
                broker.SelectAllPdsDatasAsync(),
                    Times.Once);

            this.odsStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
