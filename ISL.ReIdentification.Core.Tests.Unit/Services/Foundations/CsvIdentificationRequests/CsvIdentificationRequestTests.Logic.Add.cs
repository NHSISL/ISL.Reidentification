// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldAddCsvIdentificationRequestAsync()
        {
            //given
            CsvIdentificationRequest randomCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();
            CsvIdentificationRequest inputCsvIdentificationRequest = randomCsvIdentificationRequest;
            CsvIdentificationRequest storageCsvIdentificationRequest = inputCsvIdentificationRequest.DeepClone();
            CsvIdentificationRequest expectedCsvIdentificationRequest = inputCsvIdentificationRequest.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.InsertCsvIdentificationRequestAsync(inputCsvIdentificationRequest))
                    .ReturnsAsync(storageCsvIdentificationRequest);

            //when
            CsvIdentificationRequest actualCsvIdentificationRequest =
                await this.csvIdentificationRequestService.AddCsvIdentificationRequestAsync(inputCsvIdentificationRequest);

            //then
            actualCsvIdentificationRequest.Should().BeEquivalentTo(expectedCsvIdentificationRequest);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertCsvIdentificationRequestAsync(inputCsvIdentificationRequest),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}