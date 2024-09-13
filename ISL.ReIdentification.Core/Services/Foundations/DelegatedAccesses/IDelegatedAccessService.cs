// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses
{
    public interface IDelegatedAccessService
    {
        ValueTask<DelegatedAccess> AddDelegatedAccessAsync(DelegatedAccess delegatedAccess);
        ValueTask<IQueryable<DelegatedAccess>> RetrieveAllDelegatedAccessesAsync(DelegatedAccess delegatedAccess);
        ValueTask<DelegatedAccess> ModifyDelegatedAccessAsync(DelegatedAccess delegatedAccess);
    }
}
