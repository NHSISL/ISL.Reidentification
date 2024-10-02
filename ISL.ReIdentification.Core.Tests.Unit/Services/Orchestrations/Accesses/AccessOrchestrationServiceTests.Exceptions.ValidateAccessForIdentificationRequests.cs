// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using Moq;
using Xeptions;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationOnValidateAccessForIdentificationRequestAndLogItAsync(
            Xeption dependencyValidationException)
        {
            // given
            AccessRequest someAccessRequest =
               CreateRandomAccessRequest();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ThrowsAsync(dependencyValidationException);

            var expectedAccessOrchestrationDependencyValidationException =
                new AccessOrchestrationDependencyValidationException(
                    message: "Access orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                this.accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationDependencyValidationException
                actualAccessOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<AccessOrchestrationDependencyValidationException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationDependencyValidationException
                .Should().BeEquivalentTo(expectedAccessOrchestrationDependencyValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogErrorAsync(It.Is(SameExceptionAs(
                   expectedAccessOrchestrationDependencyValidationException))),
                       Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
