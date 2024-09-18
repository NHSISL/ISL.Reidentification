// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public interface IPdsDataService
    {
        public ValueTask<PdsData> RetrievePdsDataByIdAsync(Guid pdsDataId);
        public ValueTask<IQueryable<PdsData>> RetrieveAllPdsDataAsync();
    }
}
