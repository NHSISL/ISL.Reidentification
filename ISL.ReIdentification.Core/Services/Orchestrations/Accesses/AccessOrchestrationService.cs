// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Services.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.UserAccesses;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService : IAccessOrchestrationService
    {
        private readonly IUserAccessService userAccessService;
        private readonly IPdsDataService pdsDataService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public AccessOrchestrationService(
            IUserAccessService userAccessService,
            IPdsDataService pdsDataService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.userAccessService = userAccessService;
            this.pdsDataService = pdsDataService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AccessRequest> ProcessDelegatedAccessRequestAsync(AccessRequest accessRequest) =>
            throw new NotImplementedException();

        public ValueTask<AccessRequest> ValidateAccessForIdentificationRequestAsync(
            AccessRequest accessRequest) =>
            TryCatch(async () =>
            {
                ValidateAccessRequestIsNotNull(accessRequest);

                List<string> userOrgs =
                    await GetOrganisationsForUserAsync(accessRequest.IdentificationRequest.UserIdentifier);

                AccessRequest validatedAccessRequest = await CheckUserAccessToPatientsAsync(accessRequest, userOrgs);

                return validatedAccessRequest;
            });


        virtual internal async ValueTask<AccessRequest> CheckUserAccessToPatientsAsync(
            AccessRequest accessRequest, List<string> userOrgs)
        {
            var exceptions = new List<Exception>();

            foreach (var identificationItem in accessRequest.IdentificationRequest.IdentificationItems)
            {
                try
                {
                    identificationItem.HasAccess =
                        await UserHasAccessToPatientAsync(identificationItem.Identifier, userOrgs);
                }
                catch (Exception ex)
                {
                    var exception = ex.DeepClone() as Xeption;
                    exception.AddData("IdentificationItemError", identificationItem.RowNumber);
                    exceptions.Add(exception);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    $"Unable to validate access for {exceptions.Count} identification requests.",
                    exceptions);
            }

            return accessRequest;
        }


        // We can remove this try catch as this method does not have the responsibility of exception handling
        // We only need a logic test - use theory to cover all the allowed cases and a 2nd logic test to cover the exclusions
        // keep validation tests
        virtual internal async ValueTask<List<string>> GetOrganisationsForUserAsync(string userEmail)
        {
            await ValidateUserEmail(userEmail);
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            IQueryable<UserAccess> userAccesses =
                await this.userAccessService
                    .RetrieveAllUserAccessesAsync();

            List<string> userAccess = await userAccesses
                .Where(userAccess =>
                    userAccess.UserEmail == userEmail
                    && userAccess.ActiveFrom <= currentDateTime
                    && (userAccess.ActiveTo == null || userAccess.ActiveTo > currentDateTime))
                .Select(userAccess => userAccess.OrgCode)
                .ToListAsync();

            return userAccess;
        }


        // Drop the try catch as this method does not have the responsibility of exception handling
        // We only need a logic test - use theory to cover all the allowed cases and a 2nd logic test to cover the exclusions
        // keep validation tests
        virtual internal async ValueTask<bool> UserHasAccessToPatientAsync(string identifier, List<string> orgs)
        {
            await ValidateIdentifierAndOrgs(identifier, orgs);
            DateTimeOffset currentDateTime = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            IQueryable<PdsData> pdsDatas =
                        await this.pdsDataService.RetrieveAllPdsDatasAsync();

            bool userHasAccess =
                pdsDatas
                    .Where(pdsData =>
                            // needs to match the patient identifier
                            pdsData.PseudoNhsNumber == identifier

                        // Check the primary care provider is active
                        && (pdsData.PrimaryCareProviderBusinessEffectiveToDate == null
                            || pdsData.PrimaryCareProviderBusinessEffectiveToDate > currentDateTime)
                        && (pdsData.PrimaryCareProviderBusinessEffectiveFromDate <= currentDateTime)

                        // check that the patient is registered with an organisation
                        && (orgs.Contains(pdsData.CcgOfRegistration)
                            || orgs.Contains(pdsData.CurrentCcgOfRegistration)
                            || orgs.Contains(pdsData.CurrentIcbOfRegistration)
                            || orgs.Contains(pdsData.IcbOfRegistration))
                        )
                    .Any();

            return userHasAccess;
        }
    }
}
