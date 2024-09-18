﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Pds;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public partial class PdsDataService : IPdsDataService
    {
        private readonly IOdsStorageBroker odsStorageBroker;
        private readonly ILoggingBroker loggingBroker;

        public PdsDataService(
            IOdsStorageBroker odsStorageBroker,
            ILoggingBroker loggingBroker)
        {
            this.odsStorageBroker = odsStorageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IQueryable<PdsData>> RetrieveAllPdsDataAsync() =>
            TryCatch(this.odsStorageBroker.SelectAllPdsDatasAsync);

        public ValueTask<PdsData> RetrievePdsDataByIdAsync(Guid pdsDataId) =>
            TryCatch(async () =>
            {
                await ValidatePdsDataId(pdsDataId);
                PdsData maybePdsData = await this.odsStorageBroker.SelectPdsDataByIdAsync(pdsDataId);
                await ValidateStoragePdsData(maybePdsData, pdsDataId);

                return maybePdsData;
            });
    }
}
