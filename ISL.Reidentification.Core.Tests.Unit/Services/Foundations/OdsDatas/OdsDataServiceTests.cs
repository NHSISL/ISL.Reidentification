// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.Ods;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.OdsDatas;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.OdsDatas
{
    public partial class OdsDataServiceTests
    {
        private readonly Mock<IOdsStorageBroker> odsStorageBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IOdsDataService odsDataService;

        public OdsDataServiceTests()
        {
            this.odsStorageBroker = new Mock<IOdsStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.odsDataService = new OdsDataService(
                odsStorageBroker.Object,
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

        private SqlException CreateSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(type: typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static Filler<OdsData> CreateOdsDataFiller() =>
            new Filler<OdsData>();

        private static Expression<Func<Xeption, bool>> SameExceptionAs(
            Xeption expectedException)
        {
            return actualException =>
                actualException.SameExceptionAs(expectedException);
        }
    }
}
