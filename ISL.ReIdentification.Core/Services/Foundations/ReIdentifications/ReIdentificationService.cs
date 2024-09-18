// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
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
        public ValueTask<IdentificationRequest> ProcessReidentificationRequests(
            IdentificationRequest identificationRequests) =>
            TryCatch(async () =>
            {
                await ValidateIdentificationRequestOnProcessAsync(identificationRequests);
                List<string> nhsNumbers = await this.necsBroker.ReIdAsync(identificationRequests.Identifier);

                IdentificationRequest returnedIdentificationRequest = new IdentificationRequest
                {
                    RowNumber = identificationRequests.RowNumber,
                    UserEmail = identificationRequests.UserEmail,
                    Identifier = nhsNumbers.FirstOrDefault(),
                    HasAccess = identificationRequests.HasAccess,
                    IsReidentified = true,
                };

                return returnedIdentificationRequest;
            });
    }
}
