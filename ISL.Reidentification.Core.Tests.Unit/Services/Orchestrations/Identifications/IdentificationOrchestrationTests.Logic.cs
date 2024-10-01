// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Identifications
{
    public partial class IdentificationOrchestrationTests
    {
        [Fact]
        public async Task ShouldCreateAuditAccessRecordIfRequestItemDoesNotHaveAccessAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomGuid = Guid.NewGuid();
            IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest(false);
            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest;
            IdentificationRequest expectedIdentificationRequest = inputIdentificationRequest.DeepClone();
            expectedIdentificationRequest.IdentificationItems[0].Identifier = "0000000000";
            IdentificationItem inputIdentificationItem = inputIdentificationRequest.IdentificationItems[0];

            AccessAudit inputAccessAudit = new AccessAudit
            {
                Id = randomGuid,
                PseudoIdentifier = inputIdentificationItem.Identifier,
                UserEmail = inputIdentificationRequest.UserIdentifier,
                Reason = inputIdentificationRequest.Reason,
                HasAccess = (bool)inputIdentificationItem.HasAccess,
                Message = inputIdentificationItem.Message,
                CreatedBy = "System",
                CreatedDate = randomDateTimeOffset,
                UpdatedBy = "System",
                UpdatedDate = randomDateTimeOffset
            };

            AccessAudit outputAccessAudit = inputAccessAudit;

            this.identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ReturnsAsync(randomGuid);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            var actualIdentificationRequest =
                await this.identificationOrchestrationService
                    .ProcessIdentificationRequestAsync(inputIdentificationRequest);

            // then
            actualIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(inputIdentificationRequest.IdentificationItems.Count()));

            this.accessAuditService.Verify(service =>
                service.AddAccessAuditAsync(It.Is(SameAccessAuditAs(inputAccessAudit))),
                    Times.Exactly(inputIdentificationRequest.IdentificationItems.Count()));

            this.accessAuditService.VerifyNoOtherCalls();
            this.reIdentificationService.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldCreateAuditAccessRecordAndPerformReIdentificationIfRequestItemHasAccessAsync()
        {
            // given
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Guid randomGuid = Guid.NewGuid();
            string randomString = GetRandomStringWithLength(10);
            string reIdentifiedIdentifier = randomString;
            IdentificationRequest randomIdentificationRequest = CreateRandomIdentificationRequest(true);
            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest.DeepClone();
            IdentificationItem inputIdentificationItem = inputIdentificationRequest.IdentificationItems[0];

            AccessAudit inputAccessAudit = new AccessAudit
            {
                Id = randomGuid,
                PseudoIdentifier = inputIdentificationItem.Identifier,
                UserEmail = inputIdentificationRequest.UserIdentifier,
                Reason = inputIdentificationRequest.Reason,
                HasAccess = (bool)inputIdentificationItem.HasAccess,
                Message = inputIdentificationItem.Message,
                CreatedBy = "System",
                CreatedDate = randomDateTimeOffset,
                UpdatedBy = "System",
                UpdatedDate = randomDateTimeOffset
            };

            AccessAudit outputAccessAudit = inputAccessAudit;
            var hasAccessIdentificationItems = inputIdentificationRequest.IdentificationItems;

            IdentificationRequest inputHasAccessIdentificationRequest = new IdentificationRequest
            {
                Id = inputIdentificationRequest.Id,
                IdentificationItems = hasAccessIdentificationItems,
                UserIdentifier = inputIdentificationRequest.UserIdentifier,
                Purpose = inputIdentificationRequest.Purpose,
                Organisation = inputIdentificationRequest.Organisation,
                Reason = inputIdentificationRequest.Reason
            };

            IdentificationRequest outputHasAccessIdentificationRequest =
                inputHasAccessIdentificationRequest.DeepClone();

            outputHasAccessIdentificationRequest.IdentificationItems[0].Identifier = reIdentifiedIdentifier;

            IdentificationRequest expectedIdentificationRequest = outputHasAccessIdentificationRequest;
            //expectedIdentificationRequest.IdentificationItems =
            //    outputHasAccessIdentificationRequest.IdentificationItems;

            this.identifierBrokerMock.Setup(broker =>
               broker.GetIdentifierAsync())
                   .ReturnsAsync(randomGuid);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.reIdentificationService.Setup(service =>
                service.ProcessReidentificationRequest(inputHasAccessIdentificationRequest))
                    .ReturnsAsync(outputHasAccessIdentificationRequest);

            // when
            var actualIdentificationRequest =
                await this.identificationOrchestrationService
                    .ProcessIdentificationRequestAsync(inputIdentificationRequest);

            // then
            actualIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(inputIdentificationRequest.IdentificationItems.Count()));

            this.accessAuditService.Verify(service =>
                service.AddAccessAuditAsync(It.Is(SameAccessAuditAs(inputAccessAudit))),
                    Times.Exactly(inputIdentificationRequest.IdentificationItems.Count()));

            this.reIdentificationService.Verify(service =>
                service.ProcessReidentificationRequest(It.Is(
                    SameIdentificationRequestAs(inputHasAccessIdentificationRequest))),
                        Times.Exactly(inputIdentificationRequest.IdentificationItems.Count()));

            this.accessAuditService.VerifyNoOtherCalls();
            this.reIdentificationService.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
