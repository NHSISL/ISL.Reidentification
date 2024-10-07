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
            var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
                (this.userAccessServiceMock.Object,
                this.pdsDataServiceMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerMock.Object)
            { CallBase = true };

            string userEmail = GetRandomStringWithLength(10);
            string inputUserEmail = userEmail;
            string identifier = GetRandomStringWithLength(5);
            string inputIdentifier = identifier;
            AccessRequest accessRequest = CreateRandomAccessRequest();
            accessRequest.IdentificationRequest.UserIdentifier = userEmail;
            accessRequest.IdentificationRequest.IdentificationItems[0].Identifier = inputIdentifier;
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

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;
            AccessRequest expectedAccessRequest = accessRequest;

            // when
            AccessRequest actualAccessRequest =
                await accessOrchestrationService.ValidateAccessForIdentificationRequestAsync(accessRequest);

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

        //[Fact]
        //public async Task ShouldSetHasAccessToFalseIfUserDoesNotHaveAccessValidateAccessForIdentificationRequests()
        //{
        //    // given
        //    var accessOrchestrationServiceMock = new Mock<AccessOrchestrationService>
        //        (this.dateTimeBrokerMock.Object,
        //        this.userAccessServiceMock.Object,
        //        this.pdsDataServiceMock.Object,
        //        this.loggingBrokerMock.Object)
        //    { CallBase = true };

        //    string userEmail = GetRandomStringWithLength(10);
        //    string inputUserEmail = userEmail;
        //    string identifier = GetRandomStringWithLength(5);
        //    string inputIdentifier = identifier;
        //    AccessRequest accessRequest = CreateRandomAccessRequest();
        //    accessRequest.IdentificationRequest.UserIdentifier = userEmail;
        //    accessRequest.IdentificationRequest.IdentificationItems[0].Identifier = inputIdentifier;
        //    string userOrganisation = GetRandomStringWithLength(5);

        //    List<string> userOrganisations =
        //        new List<string> { userOrganisation };

        //    bool userHasAccessToPatientResult = false;

        //    accessOrchestrationServiceMock.Setup(service =>
        //        service.GetOrganisationsForUserAsync(inputUserEmail))
        //            .ReturnsAsync(userOrganisations);

        //    accessOrchestrationServiceMock.Setup(service =>
        //        service.UserHasAccessToPatientAsync(inputIdentifier, userOrganisations))
        //            .ReturnsAsync(userHasAccessToPatientResult);

        //    AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;
        //    AccessRequest expectedAccessRequest = accessRequest;

        //    // when
        //    AccessRequest actualAccessRequest =
        //        await accessOrchestrationService.ValidateAccessForIdentificationRequestAsync(accessRequest);

        //    // then
        //    actualAccessRequest.Should().Be(expectedAccessRequest);

        //    accessOrchestrationServiceMock.Verify(service =>
        //        service.GetOrganisationsForUserAsync(inputUserEmail),
        //            Times.Once);

        //    accessOrchestrationServiceMock.Verify(service =>
        //        service.UserHasAccessToPatientAsync(inputIdentifier, userOrganisations),
        //            Times.Once);

        //    accessOrchestrationServiceMock.VerifyNoOtherCalls();
        //}
    }
}
