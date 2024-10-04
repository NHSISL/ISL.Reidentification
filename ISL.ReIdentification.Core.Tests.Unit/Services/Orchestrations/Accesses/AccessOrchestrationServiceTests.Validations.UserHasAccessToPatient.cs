// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(GetTestInvalidParameters))]
        public async Task ShouldThrowValidationExceptionOnUserHasAccessToPatientIfInvalidAndLogItAsync(
            string invalidText,
            List<string> invalidStringList)
        {
            // Given
            string invalidIdentifier = invalidText;
            List<string> invalidOrganisations = invalidStringList;

            var invalidArgumentAccessOrchestrationException =
                new InvalidArgumentAccessOrchestrationException(
                    message: "Invalid argument access orchestration exception, " +
                        "please correct the errors and try again.");

            invalidArgumentAccessOrchestrationException.AddData(
                key: "identifier",
                values: "Text is invalid");

            invalidArgumentAccessOrchestrationException.AddData(
                key: "orgs",
                values: "List of text is invalid");

            var expectedAccessValidationOrchestrationException =
                new AccessOrchestrationValidationException(
                    message: "Access orchestration validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentAccessOrchestrationException);

            // When
            ValueTask<bool> getUserHasAccessToPatientTask = this.accessOrchestrationService
                .UserHasAccessToPatientAsync(invalidIdentifier, invalidOrganisations);

            AccessOrchestrationValidationException actualAccessValidationOrchestrationException =
                await Assert.ThrowsAsync<AccessOrchestrationValidationException>(
                    getUserHasAccessToPatientTask.AsTask);

            // Then
            actualAccessValidationOrchestrationException.Should()
                .BeEquivalentTo(expectedAccessValidationOrchestrationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogErrorAsync(It.Is(SameExceptionAs(
                    expectedAccessValidationOrchestrationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userAccessServiceMock.VerifyNoOtherCalls();
            this.pdsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
