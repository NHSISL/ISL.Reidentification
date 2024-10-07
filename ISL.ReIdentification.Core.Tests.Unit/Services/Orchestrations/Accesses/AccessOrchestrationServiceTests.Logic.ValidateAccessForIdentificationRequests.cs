﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
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
            AccessRequest randomAccessRequest = CreateRandomAccessRequest();
            AccessRequest inputAccessRequest = randomAccessRequest.DeepClone();
            inputAccessRequest.IdentificationRequest.UserIdentifier = userEmail;
            AccessRequest outputAccessRequest = inputAccessRequest.DeepClone();
            outputAccessRequest.IdentificationRequest.IdentificationItems.ForEach(x => x.HasAccess = true);
            AccessRequest expectedAccessRequest = outputAccessRequest.DeepClone();
            string userOrganisation = GetRandomStringWithLength(5);

            List<string> userOrganisations =
                new List<string> { userOrganisation };

            accessOrchestrationServiceMock.Setup(service =>
                service.GetOrganisationsForUserAsync(inputUserEmail))
                    .ReturnsAsync(userOrganisations);

            accessOrchestrationServiceMock.Setup(service =>
                service.CheckUserAccessToPatientsAsync(inputAccessRequest, userOrganisations))
                    .ReturnsAsync(outputAccessRequest);

            AccessOrchestrationService accessOrchestrationService = accessOrchestrationServiceMock.Object;

            // when
            AccessRequest actualAccessRequest =
                await accessOrchestrationService.ValidateAccessForIdentificationRequestAsync(inputAccessRequest);

            // then
            actualAccessRequest.Should().BeEquivalentTo(expectedAccessRequest);

            accessOrchestrationServiceMock.Verify(service =>
                service.GetOrganisationsForUserAsync(inputUserEmail),
                    Times.Once);

            accessOrchestrationServiceMock.Verify(service =>
                service.CheckUserAccessToPatientsAsync(inputAccessRequest, userOrganisations),
                    Times.Once);

            accessOrchestrationServiceMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userAccessServiceMock.VerifyNoOtherCalls();
            this.pdsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
