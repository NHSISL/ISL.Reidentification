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
            Guid someGuid = Guid.NewGuid();
            IdentificationRequest someIdentificationRequest = CreateRandomIdentificationRequest(false);
            IdentificationRequest inputIdentificationRequest = someIdentificationRequest;
            IdentificationRequest expectedIdentificationRequest = inputIdentificationRequest;
            IdentificationItem inputIdentificationItem = inputIdentificationRequest.IdentificationItems[0];

            AccessAudit inputAccessAudit = new AccessAudit
            {
                Id = someGuid,
                PseudoIdentifier = inputIdentificationItem.Identifier,
                UserEmail = inputIdentificationRequest.UserIdentifier,
                Reason = inputIdentificationRequest.Reason,
                HasAccess = (bool)inputIdentificationItem.HasAccess,
                Message = inputIdentificationItem.Message,
                CreatedBy = "System",
                CreatedDate = someDateTimeOffset,
                UpdatedBy = "System",
                UpdatedDate = someDateTimeOffset
            };

            AccessAudit outputAccessAudit = inputAccessAudit;

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(someGuid);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(someDateTimeOffset);

            this.accessAuditService.Setup(service =>
                service.AddAccessAuditAsync(inputAccessAudit))
                    .ReturnsAsync(outputAccessAudit);

            // when
            var returnedIdentificationRequest =
                await this.identificationOrchestrationService
                    .ProcessIdentificationRequestAsync(inputIdentificationRequest);

            // then
            returnedIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.accessAuditService.Verify(service =>
                service.AddAccessAuditAsync(It.Is(SameAccessAuditAs(inputAccessAudit))),
                    Times.Once());

            this.accessAuditService.VerifyNoOtherCalls();
            this.reIdentificationService.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
