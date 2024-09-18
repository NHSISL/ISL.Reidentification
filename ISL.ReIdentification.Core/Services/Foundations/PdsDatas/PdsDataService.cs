// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Pds;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.Pds;

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

    }
}
