// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldRetrieveOdsDataByIdAsync()
        {
            // given
            OdsData randomOdsData = CreateRandomOdsData();
            OdsData inputOdsData = randomOdsData;
            OdsData storageOdsData = inputOdsData;
            OdsData expectedOdsData = storageOdsData.DeepClone();

            this.patientOrgReferenceStorageBroker.Setup(broker =>
                broker.SelectOdsDataByIdAsync(inputOdsData.Id))
                    .ReturnsAsync(storageOdsData);

            // when
            OdsData actualOdsData = await this.odsDataService.RetrieveOdsDataByIdAsync(inputOdsData.Id);

            // then
            actualOdsData.Should().BeEquivalentTo(expectedOdsData);

            this.patientOrgReferenceStorageBroker.Verify(broker =>
                broker.SelectOdsDataByIdAsync(inputOdsData.Id),
                    Times.Once());

            this.patientOrgReferenceStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
