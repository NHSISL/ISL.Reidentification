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
        public async Task ShouldProcessReidentificationRequestsAsync()
        {
            // Given
            IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest();
            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest;
            IdentificationRequest storageIdentificationRequest = inputIdentificationRequest.DeepClone();
            IdentificationRequest expectedIdentificationRequest = storageIdentificationRequest.DeepClone();


            Mock<IReIdentificationService> reIdentificationServiceMock =
                new Mock<IReIdentificationService> { CallBase = true };

            reIdentificationServiceMock.Setup(service =>
                service.ProcessReidentificationRequest(inputIdentificationRequest))
                    .ReturnsAsync(storageIdentificationRequest);

            IReIdentificationService reIdentificationService = reIdentificationServiceMock.Object;

            // When
            IdentificationRequest actualIdentificationRequest = await reIdentificationService
                .ProcessReidentificationRequest(inputIdentificationRequest);

            // Then
            actualIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            reIdentificationServiceMock.Verify(service =>
                service.ProcessReidentificationRequest(inputIdentificationRequest),
                    Times.Once());

            this.necsBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
