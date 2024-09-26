// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.OdsDatas
{
    public interface IOdsDataService
    {
        ValueTask<IQueryable<OdsData>> RetrieveAllOdsDatasAsync();
        ValueTask<OdsData> RetrieveOdsDataByIdAsync(Guid odsId);
    }
}
