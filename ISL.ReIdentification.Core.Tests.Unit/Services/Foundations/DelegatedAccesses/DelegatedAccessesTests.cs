// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        private readonly Mock<IReIdentificationStorageBroker> ReIdentificationStorageBroker;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly DelegatedAccessService delegatedAccessService;

        public DelegatedAccessesTests()
        {
            this.ReIdentificationStorageBroker = new Mock<IReIdentificationStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.delegatedAccessService = new DelegatedAccessService(
                ReIdentificationStorageBroker.Object,
                dateTimeBrokerMock.Object,
                loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DelegatedAccess CreateRandomDelegatedAccess() =>
            CreateRandomDelegatedAccess(dateTimeOffset: GetRandomDateTimeOffset());

        private static DelegatedAccess CreateRandomDelegatedAccess(DateTimeOffset dateTimeOffset) =>
            CreateDelegatedAccessesFiller(dateTimeOffset).Create();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            return new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length)
                .GetValue();
        }

        private SqlException CreateSqlException()
        {
            return (SqlException)RuntimeHelpers.GetUninitializedObject(
                type: typeof(SqlException));
        }

        private static Filler<DelegatedAccess> CreateDelegatedAccessesFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<DelegatedAccess>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
                .OnProperty(delegatedAccess => delegatedAccess.CreatedBy).Use(user)
                .OnProperty(delegatedAccess => delegatedAccess.UpdatedBy).Use(user);

            return filler;
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException)
        {
            return actualException =>
                actualException.SameExceptionAs(expectedException);
        }
    }
}
