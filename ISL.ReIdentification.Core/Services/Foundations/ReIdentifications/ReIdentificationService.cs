// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Models.Brokers.NECS;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using LHDS.Core.Brokers.NECS;

namespace ISL.ReIdentification.Core.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationService : IReIdentificationService
    {
        private readonly INECSBroker necsBroker;
        private readonly NECSConfiguration necsConfiguration;
        private readonly ILoggingBroker loggingBroker;

        public ReIdentificationService(
            INECSBroker necsBroker,
            NECSConfiguration necsConfiguration,
            ILoggingBroker loggingBroker)
        {
            this.necsBroker = necsBroker;
            this.necsConfiguration = necsConfiguration;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IdentificationRequest> ProcessReidentificationRequest(
            IdentificationRequest identificationRequests) =>
            TryCatch(async () =>
            {
                await ValidateIdentificationRequestOnProcessAsync(identificationRequests);

                IdentificationRequest processedItems =
                    await BulkProcessRequests(identificationRequests, necsConfiguration.ApiMaxBatchSize);

                return processedItems;
            });

        virtual internal async ValueTask<IdentificationRequest> BulkProcessRequests(
            IdentificationRequest identificationRequests, int batchSize)
        {
            throw new NotImplementedException();
        }
    }
}
