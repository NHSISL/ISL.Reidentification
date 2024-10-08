// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Microsoft.EntityFrameworkCore;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference
{
    public partial class PatientOrgReferenceStorageBroker
    {
        public DbSet<PdsData> PdsDatas { get; set; }

        public async ValueTask<IQueryable<PdsData>> SelectAllPdsDatasAsync() =>
            await SelectAllAsync<PdsData>();

        public async ValueTask<PdsData> SelectPdsDataByIdAsync(long pdsDataRowId) =>
            await SelectAsync<PdsData>(pdsDataRowId);
    }
}