// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Identifications
{
    public partial class IdentificationOrchestrationTests
    {
        [Fact]
        public async Task ShouldCreateAuditAccessRecordIfRequestDoesNotHaveAccessAsync()
        {
            // given
            DateTimeOffset someDateTimeOffset = GetRandomDateTimeOffset();
            IdentificationRequest someIdentificationRequest = CreateRandomIdentificationRequest(false);
            IdentificationRequest inputIdentificationRequest = someIdentificationRequest;
            IdentificationRequest expectedIdentificationRequest = inputIdentificationRequest;
            IdentificationItem inputIdentificationItem = inputIdentificationRequest.IdentificationItems[0];

            AccessAudit inputAccessAudit = new AccessAudit
            {
                PseudoIdentifier = inputIdentificationItem.Identifier,
                UserEmail = inputIdentificationRequest.UserIdentifier,
                Reason = inputIdentificationRequest.Reason,
                HasAccess = (bool)inputIdentificationItem.HasAccess,
                Message = inputIdentificationItem.Message,
                CreatedBy = inputIdentificationRequest.Id.ToString(),
                CreatedDate = someDateTimeOffset,
                UpdatedBy = inputIdentificationRequest.Id.ToString(),
                UpdatedDate = someDateTimeOffset
            };

            AccessAudit outputAccessAudit = inputAccessAudit;

            this.accessAuditService.Setup(service =>
                service.AddAccessAuditAsync(inputAccessAudit))
                    .ReturnsAsync(outputAccessAudit);

            // when
            var returnedIdentificationRequest =
                await this.identificationOrchestrationService
                    .ProcessIdentificationRequestAsync(inputIdentificationRequest);

            // then
            returnedIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            this.accessAuditService.Verify(service =>
                service.AddAccessAuditAsync(inputAccessAudit),
                    Times.Once());

            this.reIdentificationService.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
