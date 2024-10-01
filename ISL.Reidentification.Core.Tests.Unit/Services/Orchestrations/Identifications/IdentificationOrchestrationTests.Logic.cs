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
            int itemCount = 1; //GetRandomNumber();

            IdentificationRequest randomIdentificationRequest =
               CreateRandomIdentificationRequest(hasAccess: false, itemCount: itemCount);

            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest;
            IdentificationRequest outputIdentificationRequest = inputIdentificationRequest.DeepClone();
            // outputIdentificationRequest.IdentificationItems.ForEach(item => item.Identifier = "0000000000");
            IdentificationRequest expectedIdentificationRequest = outputIdentificationRequest.DeepClone();

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
            // actualIdentificationRequest.Should().BeEquivalentTo(expectedIdentificationRequest);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Exactly(itemCount));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(itemCount));

            foreach (IdentificationItem item in inputIdentificationRequest.IdentificationItems)
            {
                AccessAudit inputAccessAudit = new AccessAudit
                {
                    Id = randomGuid,
                    PseudoIdentifier = item.Identifier,
                    UserEmail = inputIdentificationRequest.UserIdentifier,
                    Reason = inputIdentificationRequest.Reason,
                    HasAccess = (bool)item.HasAccess,
                    Message = item.Message,
                    CreatedBy = "System",
                    CreatedDate = randomDateTimeOffset,
                    UpdatedBy = "System",
                    UpdatedDate = randomDateTimeOffset
                };

                this.accessAuditService.Verify(service =>
                    service.AddAccessAuditAsync(It.Is(SameAccessAuditAs(inputAccessAudit))),
                        Times.Once);
            }

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
            int itemCount = GetRandomNumber();
            string randomString = GetRandomStringWithLength(10);
            string reIdentifiedIdentifier = randomString;

            IdentificationRequest randomIdentificationRequest =
                CreateRandomIdentificationRequest(hasAccess: true, itemCount: itemCount);

            IdentificationRequest inputIdentificationRequest = randomIdentificationRequest.DeepClone();

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
                    Times.Exactly(itemCount));

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Exactly(itemCount));

            foreach (IdentificationItem item in inputIdentificationRequest.IdentificationItems)
            {
                AccessAudit inputAccessAudit = new AccessAudit
                {
                    Id = randomGuid,
                    PseudoIdentifier = item.Identifier,
                    UserEmail = inputIdentificationRequest.UserIdentifier,
                    Reason = inputIdentificationRequest.Reason,
                    HasAccess = (bool)item.HasAccess,
                    Message = item.Message,
                    CreatedBy = "System",
                    CreatedDate = randomDateTimeOffset,
                    UpdatedBy = "System",
                    UpdatedDate = randomDateTimeOffset
                };

                this.accessAuditService.Verify(service =>
                    service.AddAccessAuditAsync(It.Is(SameAccessAuditAs(inputAccessAudit))),
                        Times.Once);
            }

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
