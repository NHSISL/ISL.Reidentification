// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Identifiers;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Services.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Services.Foundations.ReIdentifications;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Identifications
{
    public partial class IdentificationOrchestrationService : IIdentificationOrchestrationService
    {
        private readonly IReIdentificationService reIdentificationService;
        private readonly IAccessAuditService accessAuditService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IIdentifierBroker identifierBroker;

        public IdentificationOrchestrationService(
            IReIdentificationService reIdentificationService,
            IAccessAuditService accessAuditService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IIdentifierBroker identifierBroker)
        {
            this.reIdentificationService = reIdentificationService;
            this.accessAuditService = accessAuditService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.identifierBroker = identifierBroker;
        }

        public async ValueTask<IdentificationRequest> ProcessIdentificationRequestAsync(
            IdentificationRequest identificationRequest)
        {
            foreach (IdentificationItem item in identificationRequest.IdentificationItems)
            {
                var now = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
                var accessAuditId = await this.identifierBroker.GetIdentifierAsync();

                AccessAudit inputAccessAudit = new AccessAudit
                {
                    Id = accessAuditId,
                    PseudoIdentifier = item.Identifier,
                    UserEmail = identificationRequest.UserIdentifier,
                    Reason = identificationRequest.Reason,
                    HasAccess = (bool)item.HasAccess,
                    Message = item.Message,
                    CreatedBy = "System",
                    CreatedDate = now,
                    UpdatedBy = "System",
                    UpdatedDate = now
                };

                await this.accessAuditService.AddAccessAuditAsync(inputAccessAudit);

                if (item.HasAccess is false)
                {
                    item.Identifier = "0000000000";
                }
            }

            return identificationRequest;
        }

    }
}
