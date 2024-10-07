// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        //[Fact]
        //public async Task ShouldThrowServiceExceptionOnUserHasAccessToPatientIfServiceErrorOccurredAndLogItAsync()
        //{
        //    // given
        //    string someIdentifier = GetRandomStringWithLength(15);
        //    string someUserOrganisation = GetRandomStringWithLength(5);

        //    List<string> someUserOrganisations =
        //        new List<string> { someUserOrganisation };

        //    var serviceException = new Exception();

        //    var failedServiceAccessOrchestrationException =
        //        new FailedServiceAccessOrchestrationException(
        //            message: "Failed service access orchestration error occurred, contact support.",
        //            innerException: serviceException);

        //    var expectedAccessOrchestrationServiceException =
        //        new AccessOrchestrationServiceException(
        //            message: "Access orchestration service error occurred, please contact support.",
        //            innerException: failedServiceAccessOrchestrationException);

        //    this.dateTimeBrokerMock.Setup(broker =>
        //        broker.GetCurrentDateTimeOffsetAsync())
        //            .ThrowsAsync(serviceException);

        //    // when
        //    ValueTask<bool> userHasAccessToPatientTask =
        //        this.accessOrchestrationService.UserHasAccessToPatientAsync(someIdentifier, someUserOrganisations);

        //    AccessOrchestrationServiceException actualAccessOrchestrationServiceException =
        //        await Assert.ThrowsAsync<AccessOrchestrationServiceException>(
        //            testCode: userHasAccessToPatientTask.AsTask);

        //    // then
        //    actualAccessOrchestrationServiceException.Should().BeEquivalentTo(
        //        expectedAccessOrchestrationServiceException);

        //    this.dateTimeBrokerMock.Verify(broker =>
        //        broker.GetCurrentDateTimeOffsetAsync(),
        //                Times.Once);

        //    this.loggingBrokerMock.Verify(broker =>
        //        broker.LogErrorAsync(It.Is(SameExceptionAs(
        //            expectedAccessOrchestrationServiceException))),
        //                Times.Once);

        //    this.pdsDataServiceMock.Verify(broker =>
        //        broker.RetrieveAllPdsDatasAsync(),
        //            Times.Never);

        //    this.dateTimeBrokerMock.VerifyNoOtherCalls();
        //    this.loggingBrokerMock.VerifyNoOtherCalls();
        //    this.pdsDataServiceMock.VerifyNoOtherCalls();
        //}
    }
}
