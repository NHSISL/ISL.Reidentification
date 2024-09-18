// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Pds;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.PdsDatas;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.PdsDatas
{
    public partial class PdsDataServiceTests
    {
        private readonly Mock<IOdsStorageBroker> odsStorageBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly PdsDataService pdsDataService;

        public PdsDataServiceTests()
        {
            this.odsStorageBroker = new Mock<IOdsStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.pdsDataService = new PdsDataService(
                odsStorageBroker.Object,
                loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static PdsData CreateRandomPdsData() =>
            CreatePdsDataFiller().Create();

        private static IQueryable<PdsData> CreateRandomPdsDatas()
        {
            return CreatePdsDataFiller()
                .Create(GetRandomNumber())
                .AsQueryable();
        }

        private static Filler<PdsData> CreatePdsDataFiller()
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<PdsData>();

            filler.Setup();

            return filler;
        }
    }
}
