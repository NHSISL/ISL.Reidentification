// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService : IAccessOrchestrationService
    {
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IReIdentificationStorageBroker reIdentificationStorageBroker;
        private readonly IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker;
        private readonly ILoggingBroker loggingBroker;

        public AccessOrchestrationService(
            IDateTimeBroker dateTimeBroker,
            IReIdentificationStorageBroker reIdentificationStorageBroker,
            IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker,
            ILoggingBroker loggingBroker
            )
        {
            this.dateTimeBroker = dateTimeBroker;
            this.reIdentificationStorageBroker = reIdentificationStorageBroker;
            this.patientOrgReferenceStorageBroker = patientOrgReferenceStorageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AccessRequest> ProcessDelegatedAccessRequestAsync(AccessRequest accessRequest) =>
            throw new NotImplementedException();

        public ValueTask<AccessRequest> ValidateAccessForIdentificationRequestsAsync(
            AccessRequest accessRequest) =>
            TryCatch(async () =>
            {
                ValidateAccessRequestIsNotNull(accessRequest);

                List<string> userOrgs =
                    await GetOrganisationsForUserAsync(accessRequest.IdentificationRequest.UserIdentifier);

                var exceptions = new List<Exception>();

                foreach (var identificationItem in accessRequest.IdentificationRequest.IdentificationItems)
                {
                    try
                    {
                        await TryCatch(async () =>
                        {
                            identificationItem.HasAccess =
                                await UserHasAccessToPatientAsync(identificationItem.Identifier, userOrgs);
                        });
                    }
                    catch (Exception ex)
                    {
                        ((Xeption)ex).AddData("IdentificationItemError", identificationItem.RowNumber);
                        exceptions.Add(ex);
                    }
                }

                if (exceptions.Any())
                {
                    throw new AggregateException(
                        $"Unable to validate access for {exceptions.Count} identification requests.",
                        exceptions);
                }

                return accessRequest;
            });

        virtual internal ValueTask<List<string>> GetOrganisationsForUserAsync(string userEmail) =>
            TryCatch(async () =>
            {
                await ValidateUserEmail(userEmail);
                DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                IQueryable<UserAccess> userAccesses =
                    await this.reIdentificationStorageBroker.SelectAllUserAccessesAsync();

                UserAccess? maybeUserAccess =
                    userAccesses
                        .Where(userAccess => userAccess.UserEmail == userEmail)
                        .SingleOrDefault();

                List<string> userOrganisations = new List<string>();

                if (maybeUserAccess is not null)
                {
                    var userOrgCode = maybeUserAccess.OrgCode;

                    IQueryable<OdsData> odsDatas =
                        await this.patientOrgReferenceStorageBroker.SelectAllOdsDatasAsync();

                    userOrganisations =
                        odsDatas
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
            });

        virtual internal ValueTask<bool> UserHasAccessToPatientAsync(string identifier, List<string> orgs) =>
            TryCatch(async () =>
            {
                await ValidateIdentifierAndOrgs(identifier, orgs);
                DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

                IQueryable<PdsData> pdsDatas =
                            await this.patientOrgReferenceStorageBroker.SelectAllPdsDatasAsync();

                bool userHasAccess =
                    pdsDatas
                        .Where(pdsData => (pdsData.PrimaryCareProviderBusinessEffectiveToDate != null
                                && pdsData.PrimaryCareProviderBusinessEffectiveToDate > currentDateTime)
                            && (pdsData.PrimaryCareProviderBusinessEffectiveFromDate <= currentDateTime)
                            && (orgs.Contains(pdsData.CcgOfRegistration)
                                || orgs.Contains(pdsData.CurrentCcgOfRegistration)
                                || orgs.Contains(pdsData.CurrentIcbOfRegistration)
                                || orgs.Contains(pdsData.IcbOfRegistration))
                            )
                        .Any();

                return userHasAccess;
            });
    }
}
