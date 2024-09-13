using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Services.Foundations.Lookups;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly ILookupService lookupService;

        public LookupServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.lookupService = new LookupService(
                storageBroker: this.storageBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        public static TheoryData MinutesBeforeOrAfter()
        {
            int randomNumber = GetRandomNumber();
            int randomNegativeNumber = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                randomNumber,
                randomNegativeNumber
            };
        }

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static IQueryable<Lookup> CreateRandomLookups()
        {
            return CreateLookupFiller(dateTimeOffset: GetRandomDateTimeOffset())
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static Lookup CreateRandomLookup() =>
            CreateLookupFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Lookup CreateRandomLookup(DateTimeOffset dateTimeOffset) =>
            CreateLookupFiller(dateTimeOffset).Create();

        private static Filler<Lookup> CreateLookupFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<Lookup>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(lookup => lookup.CreatedBy).Use(user)
                .OnProperty(lookup => lookup.UpdatedBy).Use(user);

            // TODO: Complete the filler setup e.g. ignore related properties etc...

            return filler;
        }
    }
}