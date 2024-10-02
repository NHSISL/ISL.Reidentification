// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Coordinations.Identifications.Exceptions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Coordinations.Identifications
{
    public partial class IdentificationCoordinationTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnProcessWhenAccessRequestIsNullAndLogItAsync()
        {
            // given
            AccessRequest nullAccessRequest = null;
            AccessRequest randomAccessRequest = CreateRandomAccessRequest();

            var nullAccessRequestException =
                new NullAccessRequestException(message: "Access request is null");

            var expectedIdentificationCoordinationValidationException =
                new IdentificationCoordinationValidationException(
                    message: "Identification coordination validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: nullAccessRequestException);

            this.accessOrchestrationServiceMock.Setup(service =>
                service.ValidateAccessForIdentificationRequestsAsync(nullAccessRequest))
                    .ThrowsAsync(nullAccessRequestException);

            // when
            ValueTask<AccessRequest> accessRequestTask =
                this.identificationCoordinationService.ProcessIdentificationRequestsAsync(nullAccessRequest);

            IdentificationCoordinationValidationException actualIdentificationCoordinationValidationException =
                await Assert.ThrowsAsync<IdentificationCoordinationValidationException>(accessRequestTask.AsTask);

            // then
            actualIdentificationCoordinationValidationException
                .Should().BeEquivalentTo(expectedIdentificationCoordinationValidationException);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedIdentificationCoordinationValidationException))),
                       Times.Once);

            this.accessOrchestrationServiceMock.VerifyNoOtherCalls();
            this.identificationOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
