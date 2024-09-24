// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Pds;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public partial class PdsDataService : IPdsDataService
    {
        private readonly IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker;
        private readonly ILoggingBroker loggingBroker;

        public PdsDataService(
            IPatientOrgReferenceStorageBroker patientOrgReferenceStorageBroker,
            ILoggingBroker loggingBroker)
        {
            this.patientOrgReferenceStorageBroker = patientOrgReferenceStorageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<IQueryable<PdsData>> RetrieveAllPdsDatasAsync() =>
            TryCatch(this.patientOrgReferenceStorageBroker.SelectAllPdsDatasAsync);

        public ValueTask<PdsData> RetrievePdsDataByIdAsync(long pdsDataRowId) =>
            TryCatch(async () =>
            {
                await ValidatePdsDataRowId(pdsDataRowId);
                PdsData maybePdsData = await this.patientOrgReferenceStorageBroker.SelectPdsDataByIdAsync(pdsDataRowId);
                await ValidateStoragePdsData(maybePdsData, pdsDataRowId);

                return maybePdsData;
            });
    }
}
