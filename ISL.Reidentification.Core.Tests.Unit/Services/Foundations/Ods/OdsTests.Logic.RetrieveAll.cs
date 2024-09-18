// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Ods
{
    public partial class OdsTests
    {
        [Fact]
        public async Task ShouldRetrieveAllOdsDatasAsync()
        {
            // given
            IQueryable<OdsData> randomOdsDatas = CreateRandomOdsDatas();
            IQueryable<OdsData> storageOdsDatas = randomOdsDatas;
            IQueryable<OdsData> expectedOdsDatas = storageOdsDatas.DeepClone();

            this.odsStorageBroker.Setup(broker =>
                broker.SelectAllOdsDatasAsync())
                    .ReturnsAsync(storageOdsDatas);

            // when
            IQueryable<OdsData> actualOdsDatas = await this.odsService.RetrieveAllOdsDatasAsync();

            // then
            actualOdsDatas.Should().BeEquivalentTo(expectedOdsDatas);

            this.odsStorageBroker.Verify(broker =>
                broker.SelectAllOdsDatasAsync(),
                    Times.Once());

            this.odsStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
