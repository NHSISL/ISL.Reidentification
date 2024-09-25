// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.PdsDatas;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers
{
    public partial class ApiBroker
    {
        private const string PdsDataRelativeUrl = "api/pdsData";

        public async ValueTask<List<PdsData>> GetAllPdsDataAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<PdsData>>($"{PdsDataRelativeUrl}/");
    }
}
