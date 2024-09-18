// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using LHDS.Core.Brokers.NECS;

namespace ISL.ReIdentification.Core.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationService : IReIdentificationService
    {
        private readonly INECSBroker necsBroker;
        private readonly ILoggingBroker loggingBroker;

        public ReIdentificationService(
            INECSBroker necsBroker,
            ILoggingBroker loggingBroker)
        {
            this.necsBroker = necsBroker;
            this.loggingBroker = loggingBroker;
        }
        public async ValueTask<IdentificationRequest> ProcessReidentificationRequests(
            IdentificationRequest identificationRequests)
        {
            List<string> retrievedIdentities = await this.necsBroker.ReIdAsync(identificationRequests.Identifier);
            IdentificationRequest returnedIdentificationRequest = identificationRequests;
            returnedIdentificationRequest.Identifier = retrievedIdentities.FirstOrDefault();

            return returnedIdentificationRequest;
        }
    }
}
