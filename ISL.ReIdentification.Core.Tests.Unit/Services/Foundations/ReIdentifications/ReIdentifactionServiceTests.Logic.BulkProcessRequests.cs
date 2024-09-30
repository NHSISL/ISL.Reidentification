// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Services.Foundations.ReIdentifications;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationServiceTests
    {
        [Fact]
        public async Task ShouldBulkProcessRequestsAsync()
        {
            // Given
            int batchSize = GetRandomNumber();
            int randomCount = (batchSize * GetRandomNumber()) + GetRandomNumber();
            IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest(count: randomCount);
            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest;
            IdentificationRequest storageIdentificationRequest = inputIdentificationRequest.DeepClone();
            IdentificationRequest expectedIdentificationRequest = storageIdentificationRequest.DeepClone();

            Mock<ReIdentificationService> reIdentificationServiceMock =
                new Mock<ReIdentificationService>(
                    this.necsBrokerMock.Object,
                    this.necsConfiguration,
                    this.loggingBrokerMock.Object)
                { CallBase = true };

            ReIdentificationService service = reIdentificationServiceMock.Object;

            // When
            IdentificationRequest actualIdentificationRequest = await service
                .BulkProcessRequestsAsync(inputIdentificationRequest, batchSize);

            // Then
            actualIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);


            this.necsBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
