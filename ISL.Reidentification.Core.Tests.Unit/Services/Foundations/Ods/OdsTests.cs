// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.Ods;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Ods
{
    public partial class OdsTests
    {
        private readonly Mock<IOdsStorageBroker> odsStorageBroker;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOdsService odsService;

        public OdsTests()
        {
            this.odsStorageBroker = new Mock<IOdsStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.odsService = new OdsService(
                odsStorageBroker.Object,
                dateTimeBrokerMock.Object,
                loggingBrokerMock.Object);
        }

        private static OdsData CreateRandomOdsData() =>
            CreateOdsDataFiller().Create();

        private static IQueryable<OdsData> CreateRandomOdsDatas()
        {
            return CreateOdsDataFiller()
                .Create(GetRandomNumber())
                .AsQueryable();
        }

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static Filler<OdsData> CreateOdsDataFiller() =>
            new Filler<OdsData>();
    }
}
