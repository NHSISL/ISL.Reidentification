// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference
{
    public partial class MockPatientOrgReferenceStorageBroker
    {
        public async ValueTask<IQueryable<PdsData>> SelectAllPdsDatasAsync() =>
            new List<PdsData> { CreatePdsData() }.AsQueryable();

        public async ValueTask<PdsData> SelectPdsDataByIdAsync(Guid pdsDataId) =>
            CreatePdsData();

        private PdsData CreatePdsData() =>
            new PdsData { Id = Guid.NewGuid() };
    }
}
