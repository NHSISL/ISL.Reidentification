// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial interface IReidentificationStorageBroker
    {
        ValueTask<UserAccess> InsertUserAccessAsync(UserAccess userAccess);
        ValueTask<IQueryable<UserAccess>> SelectAllUserAccessAsync();
        ValueTask<UserAccess> SelectUserAccessByIdAsync(Guid userAccessId);
        ValueTask<UserAccess> UpdateUserAccessAsync(UserAccess userAccess);
        ValueTask<UserAccess> DeleteUserAccessAsync(UserAccess userAccess);
    }
}
