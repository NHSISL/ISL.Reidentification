// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using Microsoft.EntityFrameworkCore;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference
{
    public partial class PatientOrgReferenceStorageBroker
    {
        public DbSet<OdsData> OdsDatas { get; set; }

        public async ValueTask<IQueryable<OdsData>> SelectAllOdsDatasAsync() =>
            await SelectAllAsync<OdsData>();

        public async ValueTask<OdsData> SelectOdsDataByIdAsync(Guid odsDataId) =>
            await SelectAsync<OdsData>(odsDataId);
    }
}