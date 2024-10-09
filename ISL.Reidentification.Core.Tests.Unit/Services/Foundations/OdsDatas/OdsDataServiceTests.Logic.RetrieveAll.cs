// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.OdsDatas
{
    public partial class OdsDataServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAllOdsDatasAsync()
        {
            // given
            IQueryable<OdsData> randomOdsDatas = CreateRandomOdsDatas();
            IQueryable<OdsData> storageOdsDatas = randomOdsDatas;
            IQueryable<OdsData> expectedOdsDatas = storageOdsDatas.DeepClone();

            this.patientOrgReferenceStorageBroker.Setup(broker =>
                broker.SelectAllOdsDatasAsync())
                    .ReturnsAsync(storageOdsDatas);

            // when
            IQueryable<OdsData> actualOdsDatas = await this.odsDataService.RetrieveAllOdsDatasAsync();

            // then
            actualOdsDatas.Should().BeEquivalentTo(expectedOdsDatas);

            this.patientOrgReferenceStorageBroker.Verify(broker =>
                broker.SelectAllOdsDatasAsync(),
                    Times.Once());

            this.patientOrgReferenceStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
