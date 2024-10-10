// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.CsvIdentificationRequests
{
    public partial class CsvIdentificationRequestsTests
    {
        [Fact]
        public async Task ShouldModifyCsvIdentificationRequestAsync()
        {
            //given
            DateTimeOffset randomDateOffset = GetRandomDateTimeOffset();

            CsvIdentificationRequest randomCsvIdentificationRequest = CreateRandomModifyCsvIdentificationRequest(randomDateOffset);
            CsvIdentificationRequest inputCsvIdentificationRequest = randomCsvIdentificationRequest.DeepClone();
            CsvIdentificationRequest storageCsvIdentificationRequest = randomCsvIdentificationRequest.DeepClone();
            storageCsvIdentificationRequest.UpdatedDate = storageCsvIdentificationRequest.CreatedDate;
            CsvIdentificationRequest updatedCsvIdentificationRequest = inputCsvIdentificationRequest.DeepClone();
            CsvIdentificationRequest expectedCsvIdentificationRequest = updatedCsvIdentificationRequest.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateOffset);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectCsvIdentificationRequestByIdAsync(inputCsvIdentificationRequest.Id))
                    .ReturnsAsync(storageCsvIdentificationRequest);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.UpdateCsvIdentificationRequestAsync(inputCsvIdentificationRequest))
                    .ReturnsAsync(updatedCsvIdentificationRequest);

            //when
            CsvIdentificationRequest actualCsvIdentificationRequest =
                await this.csvIdentificationRequestService.ModifyCsvIdentificationRequestAsync(inputCsvIdentificationRequest);

            //then
            actualCsvIdentificationRequest.Should().BeEquivalentTo(expectedCsvIdentificationRequest);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectCsvIdentificationRequestByIdAsync(inputCsvIdentificationRequest.Id),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.UpdateCsvIdentificationRequestAsync(inputCsvIdentificationRequest),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}