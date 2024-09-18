// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.Lookups;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string userAccessesRelativeUrl = "api/useraccesses";

        public async ValueTask<UserAccess> PostUserAccessAsync(UserAccess userAccess) =>
            throw new NotImplementedException();

        public async ValueTask<UserAccess> GetUserAccessByIdAsync(Guid userAccessId) =>
            throw new NotImplementedException();

        public async ValueTask<UserAccess> DeleteUserAccessByIdAsync(Guid userAccessId) =>
            throw new NotImplementedException();
    }
}
