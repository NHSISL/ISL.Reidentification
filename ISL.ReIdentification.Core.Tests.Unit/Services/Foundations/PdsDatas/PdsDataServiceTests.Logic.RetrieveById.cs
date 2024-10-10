﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.PdsDatas
{
    public partial class PdsDataServiceTests
    {
        [Fact]
        public async Task ShouldRetrievePdsDataByIdAsync()
        {
            // given
            PdsData randomPdsData = CreateRandomPdsData();
            PdsData inputPdsData = randomPdsData;
            PdsData storagePdsData = randomPdsData;
            PdsData expectedPdsData = storagePdsData.DeepClone();

            this.reIdentificationStorageBrokerMock.Setup(broker =>
                broker.SelectPdsDataByIdAsync(inputPdsData.RowId))
                    .ReturnsAsync(storagePdsData);

            // when
            PdsData actualPdsData =
                await this.pdsDataService.RetrievePdsDataByIdAsync(inputPdsData.RowId);

            // then
            actualPdsData.Should().BeEquivalentTo(expectedPdsData);

            this.reIdentificationStorageBrokerMock.Verify(broker =>
                broker.SelectPdsDataByIdAsync(inputPdsData.RowId),
                    Times.Once);

            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
