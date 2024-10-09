// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.OdsDatas
{
    public partial class OdsDataService : IOdsDataService
    {
        private readonly IReIdentificationStorageBroker reIdentificationStorageBroker;
        private readonly ILoggingBroker loggingBroker;

        public OdsDataService(
            IReIdentificationStorageBroker reIdentificationStorageBroker,
            ILoggingBroker loggingBroker)
        {
            this.reIdentificationStorageBroker = reIdentificationStorageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IQueryable<OdsData>> RetrieveAllOdsDatasAsync() =>
        TryCatch(async () =>
        {
            return await this.reIdentificationStorageBroker.SelectAllOdsDatasAsync();
        });

        public ValueTask<OdsData> RetrieveOdsDataByIdAsync(Guid odsId) =>
        TryCatch(async () =>
        {
            await ValidateOdsDataId(odsId);
            OdsData maybeOdsData = await this.reIdentificationStorageBroker.SelectOdsDataByIdAsync(odsId);
            await ValidateStorageOdsData(maybeOdsData, odsId);

            return maybeOdsData;
        });
    }
}
