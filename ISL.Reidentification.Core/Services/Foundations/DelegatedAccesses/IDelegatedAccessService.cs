// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses
{
    public interface IDelegatedAccessService
    {
        ValueTask<DelegatedAccess> AddDelegatedAccessAsync(DelegatedAccess delegatedAccess);
        ValueTask<DelegatedAccess> RetrieveDelegatedAccessByIdAsync(Guid delegatedAccessId);
        ValueTask<DelegatedAccess> ModifyDelegatedAccessAsync(DelegatedAccess delegatedAccess);
    }
}
