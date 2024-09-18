// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.Ods
{
    public partial class OdsService : IOdsService
    {
        private readonly IOdsStorageBroker odsStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public OdsService(
            IOdsStorageBroker odsStorageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.odsStorageBroker = odsStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IQueryable<OdsData>> RetrieveAllOdsDatasAsync()
        {
            return this.odsStorageBroker.SelectAllOdsDatasAsync();
        }
    }
}
