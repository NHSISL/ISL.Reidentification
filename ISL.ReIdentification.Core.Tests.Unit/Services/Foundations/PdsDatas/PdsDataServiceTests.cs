// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.PatientOrgReference;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.PdsDatas;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.PdsDatas
{
    public partial class PdsDataServiceTests
    {
        private readonly Mock<IPatientOrgReferenceStorageBroker> patientOrgReferenceStorageBroker;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly PdsDataService pdsDataService;

        public PdsDataServiceTests()
        {
            this.patientOrgReferenceStorageBroker = new Mock<IPatientOrgReferenceStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.pdsDataService = new PdsDataService(
                patientOrgReferenceStorageBroker.Object,
                loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static PdsData CreateRandomPdsData() =>
            CreateRandomPdsData(dateTimeOffset: GetRandomDateTimeOffset());

        private static PdsData CreateRandomPdsData(DateTimeOffset dateTimeOffset) =>
            CreatePdsDataFiller(dateTimeOffset).Create();

        private static IQueryable<PdsData> CreateRandomPdsDatas()
        {
            return CreatePdsDataFiller(GetRandomDateTimeOffset())
                .Create(GetRandomNumber())
                .AsQueryable();
        }

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Filler<PdsData> CreatePdsDataFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<PdsData>();

            filler.Setup()
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(pdsData => pdsData.OdsDatas).IgnoreIt();

            return filler;
        }

        private SqlException CreateSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(type: typeof(SqlException));

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException)
        {
            return actualException =>
                actualException.SameExceptionAs(expectedException);
        }
    }
}
