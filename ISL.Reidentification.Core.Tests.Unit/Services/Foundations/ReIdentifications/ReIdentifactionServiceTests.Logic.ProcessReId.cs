// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationServiceTests
    {
        [Fact(Skip = "Removed to allow logic change")]
        public async Task ShouldProcessReidentificationRequestsAsync()
        {
            //// Given
            //IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest();
            //IdentificationRequest inputIdentificationRequest = randomIdentificationRequest.DeepClone();
            //IdentificationRequest storageIdentificationRequest = inputIdentificationRequest.DeepClone();
            //storageIdentificationRequest.Identifier = GetRandomString();
            //List<string> storageIdentities = new List<string> { storageIdentificationRequest.Identifier };
            //IdentificationRequest expectedIdentificationRequest = storageIdentificationRequest.DeepClone();
            //expectedIdentificationRequest.IsReidentified = true;

            //this.necsBrokerMock.Setup(broker =>
            //    broker.ReIdAsync(inputIdentificationRequest.Identifier))
            //        .ReturnsAsync(storageIdentities);

            //// When
            //IdentificationRequest actualIdentificationRequest = await this.reIdentificationService
            //    .ProcessReidentificationRequest(inputIdentificationRequest);

            //// Then
            //actualIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            //this.necsBrokerMock.Verify(broker =>
            //    broker.ReIdAsync(inputIdentificationRequest.Identifier), 
            //        Times.Once());

            //this.necsBrokerMock.VerifyNoOtherCalls();
            //this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
