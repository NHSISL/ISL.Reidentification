// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ISL.Reidentification.Core.Brokers.DateTimes;
using ISL.Reidentification.Core.Brokers.Loggings;
using ISL.Reidentification.Core.Brokers.Storages.Sql.Reidentifications;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        private readonly Mock<IReidentificationStorageBroker> reidentificationStorageBroker;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly DelegatedAccessService delegatedAccessService;

        public DelegatedAccessesTests()
        {
            this.reidentificationStorageBroker = new Mock<IReidentificationStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.delegatedAccessService = new DelegatedAccessService(
                reidentificationStorageBroker.Object,
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

        private static Expression<Func<Xeption, bool>> SameExceptionAs(
            Xeption expectedException)
        {
            return actualException =>
                actualException.SameExceptionAs(expectedException);
        }
    }
}
