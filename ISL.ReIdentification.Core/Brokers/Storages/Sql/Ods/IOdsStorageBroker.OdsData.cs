﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods
{
    public partial interface IOdsStorageBroker
    {
        ValueTask<IQueryable<OdsData>> SelectAllOdsDatasAsync();
        ValueTask<OdsData> SelectOdsDataByIdAsync(Guid odsDataId);
    }
}
