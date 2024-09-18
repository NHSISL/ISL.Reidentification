// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;

namespace ISL.ReIdentification.Core.Services.Foundations.Ods
{
    public interface IOdsService
    {
        ValueTask<IQueryable<OdsData>> RetrieveAllOdsDatasAsync();
    }
}
