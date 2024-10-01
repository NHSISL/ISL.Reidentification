// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Services.Orchestrations.Accesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldValidateAccessForIdentificationRequests()
        {
            // given
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService> { CallBase = true };
            string userEmail = GetRandomStringWithLength(10);
            string inputUserEmail = userEmail;
            string identifier = GetRandomStringWithLength(5);
            string inputIdentifier = identifier;
            AccessRequest accessRequest = CreateRandomAccessRequest();
            string userOrganisation = GetRandomStringWithLength(5);

            List<string> userOrganisations =
                new List<string> { userOrganisation };

            bool userHasAccessToPatientResult = true;

            accessOrchestrationServiceMock.Setup(service =>
                service.GetOrganisationsForUserAsync(inputUserEmail))
                    .ReturnsAsync(userOrganisations);

            accessOrchestrationServiceMock.Setup(service =>
                service.UserHasAccessToPatientAsync(inputIdentifier, userOrganisations))
                    .ReturnsAsync(userHasAccessToPatientResult);

            AccessRequest expectedAccessRequest = accessRequest;

            // when
            AccessRequest actualAccessRequest =
                await this.accessOrchestrationService.ValidateAccessForIdentificationRequestsAsync(accessRequest);

            // then
            actualAccessRequest.Should().Be(expectedAccessRequest);

            accessOrchestrationServiceMock.Verify(service =>
                service.GetOrganisationsForUserAsync(inputUserEmail),
                    Times.Once);

            accessOrchestrationServiceMock.Verify(service =>
                service.UserHasAccessToPatientAsync(inputIdentifier, userOrganisations),
                    Times.Once);

            accessOrchestrationServiceMock.VerifyNoOtherCalls();
        }
    }
}
