// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.Pds
{
    public partial interface IOdsStorageBroker
    {
        ValueTask<IQueryable<PdsData>> SelectAllPdsDatasAsync();
        ValueTask<PdsData> SelectPdsDataByIdAsync(Guid pdsDataId);
    }
}
