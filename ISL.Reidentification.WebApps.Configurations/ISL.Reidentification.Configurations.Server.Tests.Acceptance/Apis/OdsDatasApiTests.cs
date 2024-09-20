// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.OdsDatas;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis
{
    [Collection(nameof(ApiTestCollection))]
    public partial class OdsDatasApiTests
    {
        private readonly ApiBroker apiBroker;
        private readonly IOdsStorageBroker odsStorageBrokerMock;

        public OdsDatasApiTests(ApiBroker apiBroker
            //IOdsStorageBroker odsStorageBroker
            )
        {
            this.apiBroker = apiBroker;
            //this.odsStorageBrokerMock = this.apiBroker.;
        }


        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static OdsData CreateRandomOdsData() =>
            CreateRandomOdsDataFiller().Create();

        private static Filler<OdsData> CreateRandomOdsDataFiller() =>
            new Filler<OdsData>();

        private List<OdsData> CreateRandomOdsDatas()
        {
            int randomNumber = GetRandomNumber();
            List<OdsData> randomOdsDatas = new List<OdsData>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomOdsDatas.Add(CreateRandomOdsData());
            }

            return randomOdsDatas;
        }
    }
}
