﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Coordinations.Identifications
{
    public partial class IdentificationCoordinationTests
    {
        [Fact]
        public async Task ShouldProcessIdentificationRequestsAsync()
        {
            // given
            AccessRequest randomAccessRequest = CreateRandomAccessRequest();
            IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest();
            AccessRequest inputAccessRequest = randomAccessRequest;
            AccessRequest outputOrchestrationAccessRequest = CreateRandomAccessRequest();
            IdentificationRequest inputIdentificationRequest = outputOrchestrationAccessRequest.IdentificationRequest;
            IdentificationRequest outputIdentificationRequest = randomIdentificationRequest;
            outputOrchestrationAccessRequest.IdentificationRequest = outputIdentificationRequest;
            AccessRequest expectedAccessRequest = outputOrchestrationAccessRequest.DeepClone();

            this.accessOrchestrationServiceMock.Setup(service =>
                service.ValidateAccessForIdentificationRequestsAsync(inputAccessRequest))
                    .ReturnsAsync(outputOrchestrationAccessRequest);

            this.identificationOrchestrationServiceMock.Setup(service =>
                service.ProcessIdentificationRequestAsync(inputIdentificationRequest))
                    .ReturnsAsync(outputIdentificationRequest);

            // when
            var actualAccessRequest =
                this.identificationCoordinationService.ProcessIdentificationRequestsAsync(inputAccessRequest);

            // then
            actualAccessRequest.Should().BeEquivalentTo(expectedAccessRequest);

            this.accessOrchestrationServiceMock.Verify(service =>
                service.ValidateAccessForIdentificationRequestsAsync(inputAccessRequest),
                    Times.Once);

            this.identificationOrchestrationServiceMock.Verify(service =>
                service.ProcessIdentificationRequestAsync(inputIdentificationRequest),
                    Times.Once);

            this.accessOrchestrationServiceMock.VerifyNoOtherCalls();
            this.identificationOrchestrationServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }


    }
}
