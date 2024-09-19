// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.OdsDatas
{
    public partial class OdsDataService : IOdsDataService
    {
        private readonly IOdsStorageBroker odsStorageBroker;
        private readonly ILoggingBroker loggingBroker;

        public OdsDataService(
            IOdsStorageBroker odsStorageBroker,
            ILoggingBroker loggingBroker)
        {
            this.odsStorageBroker = odsStorageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IQueryable<OdsData>> RetrieveAllOdsDatasAsync() =>
        TryCatch(async () =>
        {
            return await this.odsStorageBroker.SelectAllOdsDatasAsync();
        });

        public ValueTask<OdsData> RetrieveOdsDataByIdAsync(Guid odsId) =>
        TryCatch(async () =>
        {
            await ValidateOdsDataId(odsId);

            return await this.odsStorageBroker.SelectOdsDataByIdAsync(odsId);
        });
    }
}
