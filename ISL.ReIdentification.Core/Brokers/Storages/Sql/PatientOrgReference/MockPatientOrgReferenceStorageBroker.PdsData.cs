// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference
{
    public partial class MockPatientOrgReferenceStorageBroker
    {
        public async ValueTask<IQueryable<PdsData>> SelectAllPdsDatasAsync() =>
            new List<PdsData> { CreatePdsData() }.AsQueryable();

        public async ValueTask<PdsData> SelectPdsDataByIdAsync(long pdsDataId) =>
            CreatePdsData();

        private PdsData CreatePdsData() =>
            new PdsData { RowId = GetRandomNumber() };

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();
    }
}
