// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Services.Orchestrations.Accesses;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Identifications
{
    public partial class IdentificationCoordinationService : IIdentificationCoordinationService
    {
        private readonly IAccessOrchestrationService accessOrchestrationService;
        private readonly IIdentificationOrchestrationService identificationOrchestrationService;
        private readonly ILoggingBroker loggingBroker;

        public IdentificationCoordinationService(
            IAccessOrchestrationService accessOrchestrationService,
            IIdentificationOrchestrationService identificationOrchestrationService,
            ILoggingBroker loggingBroker)
        {
            this.accessOrchestrationService = accessOrchestrationService;
            this.identificationOrchestrationService = identificationOrchestrationService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AccessRequest> ProcessDelegatedAccessRequestAsync(AccessRequest accessRequest) =>
            throw new System.NotImplementedException();

        public async ValueTask<AccessRequest> ProcessIdentificationRequestsAsync(AccessRequest accessRequest)
        {
            var returnedAccessRequest =
                await this.accessOrchestrationService.ValidateAccessForIdentificationRequestsAsync(accessRequest);

            var returnedIdentificationRequest =
                await this.identificationOrchestrationService
                    .ProcessIdentificationRequestAsync(returnedAccessRequest.IdentificationRequest);

            returnedAccessRequest.IdentificationRequest = returnedIdentificationRequest;

            return returnedAccessRequest;
        }
    }
}
