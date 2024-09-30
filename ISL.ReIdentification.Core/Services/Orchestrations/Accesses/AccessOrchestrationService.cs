// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService : IAccessOrchestrationService
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IReIdentificationStorageBroker reIdentificationStorageBroker;
        private readonly IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker;

        public AccessOrchestrationService(
            IDateTimeBroker dateTimeBroker,
            IReIdentificationStorageBroker reIdentificationStorageBroker,
            IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker
            )
        {
            this.dateTimeBroker = dateTimeBroker;
            this.reIdentificationStorageBroker = reIdentificationStorageBroker;
            this.patientOrgReferenceStorageBroker = patientOrgReferenceStorageBroker;
        }

        public ValueTask<AccessRequest> ProcessDelegatedAccessRequestAsync(AccessRequest accessRequest) =>
            throw new NotImplementedException();

        public ValueTask<AccessRequest> ValidateAccessForIdentificationRequestsAsync(AccessRequest accessRequest) =>
            throw new NotImplementedException();

        virtual internal async ValueTask<List<string>> GetOrganisationsForUserAsync(string userEmail)
        {
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            UserAccess? maybeUserAccess =
                this.reIdentificationStorageBroker.SelectAllUserAccessesAsync().Result
                    .Where(userAccess => userAccess.UserEmail == userEmail)
                    .SingleOrDefault();

            List<string> userOrganisations = new List<string>();

            if (maybeUserAccess is not null)
            {
                var userOrgCode = maybeUserAccess.OrgCode;

                userOrganisations =
                    this.patientOrgReferenceStorageBroker.SelectAllOdsDatasAsync().Result
                        .Where(odsData => odsData.RelationshipEndDate > currentDateTime
                            && odsData.RelationshipStartDate <= currentDateTime
                            &&
                                (odsData.OrganisationCode_Root == userOrgCode
                                    || odsData.OrganisationCode_Parent == userOrgCode
                                )
                            )
                        .Select(odsData => odsData.OrganisationCode_Root)
                        .ToList();
            }

            return userOrganisations;
        }

        virtual internal ValueTask<bool> UserHasAccessToPatientAsync(string identifier, List<string> orgs)
        {
            throw new NotImplementedException();
        }
    }
}
