// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public interface IPdsDataService
    {
        public ValueTask<IQueryable<PdsData>> RetrieveAllPdsDatasAsync();
        public ValueTask<PdsData> RetrievePdsDataByIdAsync(Guid pdsDataId);
    }
}
