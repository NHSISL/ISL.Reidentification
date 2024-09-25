// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis.PdsDatas
{
    [Collection(nameof(ApiTestCollection))]
    public partial class PdsDataApiTests
    {
        private readonly ApiBroker apiBroker;

        public PdsDataApiTests(ApiBroker apiBroker)
        {
            this.apiBroker = apiBroker;
        }
    }
}
