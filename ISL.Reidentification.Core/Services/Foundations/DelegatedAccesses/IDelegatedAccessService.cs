// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses
{
    public interface IDelegatedAccessService
    {
        ValueTask<DelegatedAccess> AddDelegatedAccessAsync(DelegatedAccess delegatedAccess);
    }
}
