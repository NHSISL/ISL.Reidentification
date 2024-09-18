// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using Moq;

namespace LHDS.Core.Tests.Unit.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationServiceTests
    {
        [Fact]
        public async Task ShouldProcessReidentificationRequestsAsync()
        {
            // Given
            IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest();
            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest;
            IdentificationRequest storageIdentificationRequest = randomIdentificationRequest;
            storageIdentificationRequest.Identifier = GetRandomString();
            List<string> storageIdentities = new List<string> { storageIdentificationRequest.Identifier };
            IdentificationRequest expectedIdentificationRequest = storageIdentificationRequest;

            this.necsBrokerMock.Setup(broker =>
                broker.ReIdAsync(inputIdentificationRequest.Identifier))
                    .ReturnsAsync(storageIdentities);

            // When
            IdentificationRequest actualIdentificationRequest = await this.reIdentificationService
                .ProcessReidentificationRequests(inputIdentificationRequest);

            // Then
            actualIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            this.necsBrokerMock.Verify(broker =>
                broker.ReIdAsync(inputIdentificationRequest.Identifier), 
                    Times.Once());

            this.necsBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
