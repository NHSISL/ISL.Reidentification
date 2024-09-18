// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.DelegatedAccesses;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string DelegatedAccessesRelativeUrl = "api/delegatedAccesses";

        public async ValueTask<DelegatedAccess> PostDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            await this.apiFactoryClient.PostContentAsync(DelegatedAccessesRelativeUrl, delegatedAccess);

        public async ValueTask<DelegatedAccess> GetDelegatedAccessByIdAsync(Guid delegatedAccessId) =>
            await this.apiFactoryClient
                .GetContentAsync<DelegatedAccess>($"{DelegatedAccessesRelativeUrl}/{delegatedAccessId}");

        public async ValueTask<DelegatedAccess> DeleteDelegatedAccessByIdAsync(Guid delegatedAccessId) =>
            await this.apiFactoryClient.DeleteContentAsync<DelegatedAccess>($"{DelegatedAccessesRelativeUrl}/{delegatedAccessId}");

        public async ValueTask<DelegatedAccess> PutDelegatedAccessAsync(DelegatedAccess delegatedAccess) =>
            await this.apiFactoryClient.PutContentAsync<DelegatedAccess>(DelegatedAccessesRelativeUrl, delegatedAccess);
    }
}
