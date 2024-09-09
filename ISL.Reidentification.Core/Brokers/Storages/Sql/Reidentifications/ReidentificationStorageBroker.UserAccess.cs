// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using System;
using EFxceptions;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using Microsoft.EntityFrameworkCore;

namespace ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications
{
    public partial class ReidentificationStorageBroker : EFxceptionsContext, IReidentificationStorageBroker
    {
        public DbSet<UserAccess> UserAccesses { get; set; }
        public async ValueTask<UserAccess> InsertUserAccessAsync(UserAccess userAccess) =>
            await InsertAsync<UserAccess>(userAccess);
        public async ValueTask<IQueryable<UserAccess>> SelectAllUserAccessAsync() =>
            await SelectAllAsync<UserAccess>();
        public async ValueTask<UserAccess> SelectUserAccessByIdAsync(Guid userAccessId) =>
            await SelectAsync<UserAccess>(userAccessId);
        public async ValueTask<UserAccess> UpdateUserAccessAsync(UserAccess userAccess) =>
            await UpdateAsync<UserAccess>(userAccess);
        public async ValueTask<UserAccess> DeleteUserAccessAsync(UserAccess userAccess) =>
            await DeleteAsync<UserAccess>(userAccess);
    }
}