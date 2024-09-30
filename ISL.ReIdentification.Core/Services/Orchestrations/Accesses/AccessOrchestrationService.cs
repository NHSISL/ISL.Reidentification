// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService : IAccessOrchestrationService
    {
        private readonly IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker;
        private readonly IReIdentificationStorageBroker reIdentificationStorageBroker;

        public AccessOrchestrationService(
            IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker,
            IReIdentificationStorageBroker reIdentificationStorageBroker)
        {
            this.patientOrgReferenceStorageBroker = patientOrgReferenceStorageBroker;
            this.reIdentificationStorageBroker = reIdentificationStorageBroker;
        }

        public ValueTask<AccessRequest> ProcessDelegatedAccessRequestAsync(AccessRequest accessRequest) =>
            throw new NotImplementedException();

        public ValueTask<AccessRequest> ValidateAccessForIdentificationRequestsAsync(AccessRequest accessRequest) =>
            throw new NotImplementedException();

        virtual internal ValueTask<List<string>> GetOrganisationsForUserAsync(string userEmail)
        {
            throw new NotImplementedException();
        }

        virtual internal ValueTask<bool> UserHasAccessToPatientAsync(string identifier, List<string> orgs)
        {
            throw new NotImplementedException();
        }
    }
}
