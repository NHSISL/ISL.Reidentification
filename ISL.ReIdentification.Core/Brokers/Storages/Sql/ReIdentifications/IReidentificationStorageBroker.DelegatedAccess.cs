// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;

namespace ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications
{
    public partial interface IReIdentificationStorageBroker
    {
        ValueTask<DelegatedAccess> InsertDelegatedAccessAsync(DelegatedAccess delegatedAccess);
        ValueTask<IQueryable<DelegatedAccess>> SelectAllDelegatedAccessesAsync();
        ValueTask<DelegatedAccess> SelectDelegatedAccessByIdAsync(Guid delegateAccessId);
        ValueTask<DelegatedAccess> UpdateDelegatedAccessAsync(DelegatedAccess delegatedAccess);
        ValueTask<DelegatedAccess> DeleteDelegatedAccessAsync(DelegatedAccess delegatedAccess);
    }
}
