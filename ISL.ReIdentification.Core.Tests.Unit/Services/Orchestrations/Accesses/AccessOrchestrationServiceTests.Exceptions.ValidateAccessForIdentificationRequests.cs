// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using ISL.ReIdentification.Core.Services.Orchestrations.Accesses;
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
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.dateTimeBrokerMock.Object,
                this.reIdentificationStorageBrokerMock.Object,
                this.patientOrgReferenceStorageBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            AccessRequest someAccessRequest = CreateRandomAccessRequest();

            accessOrchestrationServiceMock.Setup(service =>
                 service.GetOrganisationsForUserAsync(It.IsAny<string>()))
                     .ThrowsAsync(dependencyValidationException);

            var expectedAccessOrchestrationDependencyValidationException =
                new AccessOrchestrationDependencyValidationException(
                    message: "Access orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: dependencyValidationException.InnerException as Xeption);

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            // when
            ValueTask<AccessRequest> validateAccessForIdentificationRequestTask =
                accessOrchestrationService
                    .ValidateAccessForIdentificationRequestsAsync(someAccessRequest);

            AccessOrchestrationDependencyValidationException
                actualAccessOrchestrationDependencyValidationException =
                await Assert.ThrowsAsync<AccessOrchestrationDependencyValidationException>(
                    validateAccessForIdentificationRequestTask.AsTask);

            // then
            actualAccessOrchestrationDependencyValidationException
                .Should().BeEquivalentTo(expectedAccessOrchestrationDependencyValidationException);

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
