// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ISL.ReIdentification.Core.Brokers.DateTimes;
using ISL.ReIdentification.Core.Brokers.Loggings;
using ISL.ReIdentification.Core.Brokers.Storages.Sql.ReIdentifications;
<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Services.Foundations.UserAccesses;
=======
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using ISL.ReIdentification.Core.Services.Foundations.ImpersonationContexts;
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.UserAccesses
{
    public partial class UserAccessesTests
=======
namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.ImpersonationContexts
{
    public partial class ImpersonationContextsTests
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs
    {
        private readonly Mock<IReIdentificationStorageBroker> reIdentificationStorageBroker;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
        private readonly UserAccessService userAccessService;

        public UserAccessesTests()
=======
        private readonly ImpersonationContextService impersonationContextService;

        public ImpersonationContextsTests()
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs
        {
            this.reIdentificationStorageBroker = new Mock<IReIdentificationStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
            this.userAccessService = new UserAccessService(
=======
            this.impersonationContextService = new ImpersonationContextService(
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs
                reIdentificationStorageBroker.Object,
                dateTimeBrokerMock.Object,
                loggingBrokerMock.Object);
        }

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
        private static UserAccess CreateRandomUserAccess() =>
            CreateRandomUserAccess(dateTimeOffset: GetRandomDateTimeOffset());

        private static UserAccess CreateRandomUserAccess(DateTimeOffset dateTimeOffset) =>
            CreateUserAccessesFiller(dateTimeOffset).Create();

        private static IQueryable<UserAccess> CreateRandomUserAccesses()
        {
            return CreateUserAccessesFiller(GetRandomDateTimeOffset())
=======
        private static ImpersonationContext CreateRandomImpersonationContext() =>
            CreateRandomImpersonationContext(dateTimeOffset: GetRandomDateTimeOffset());

        private static ImpersonationContext CreateRandomImpersonationContext(DateTimeOffset dateTimeOffset) =>
            CreateImpersonationContextsFiller(dateTimeOffset).Create();

        private static IQueryable<ImpersonationContext> CreateRandomImpersonationContexts()
        {
            return CreateImpersonationContextsFiller(GetRandomDateTimeOffset())
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs
                .Create(GetRandomNumber())
                .AsQueryable();
        }

<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
=======
        private static ImpersonationContext CreateRandomModifyImpersonationContext(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInThePast = GetRandomNegativeNumber();
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext(dateTimeOffset);

            randomImpersonationContext.CreatedDate = dateTimeOffset.AddDays(randomDaysInThePast);

            return randomImpersonationContext;
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs
        private static string GetRandomStringWithLengthOf(int length)
        {
            return new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length)
                .GetValue();
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomStringWithLength(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 2, max: 10).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private SqlException CreateSqlException() =>
            (SqlException)RuntimeHelpers.GetUninitializedObject(type: typeof(SqlException));

<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
        private static UserAccess CreateRandomModifyUserAccess(DateTimeOffset dateTimeOffset)
        {
            int randomDaysInThePast = GetRandomNegativeNumber();
            UserAccess randomUserAccess = CreateRandomUserAccess(dateTimeOffset);
            randomUserAccess.CreatedDate = dateTimeOffset.AddDays(randomDaysInThePast);

            return randomUserAccess;
        }

        private static Filler<UserAccess> CreateUserAccessesFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<UserAccess>();
=======
        private static Filler<ImpersonationContext> CreateImpersonationContextsFiller(DateTimeOffset dateTimeOffset)
        {
            string user = Guid.NewGuid().ToString();
            var filler = new Filler<ImpersonationContext>();
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnType<DateTimeOffset?>().Use(dateTimeOffset)
<<<<<<< HEAD:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/UserAccesses/UserAccessesTests.cs
                .OnProperty(userAccess => userAccess.CreatedBy).Use(user)
                .OnProperty(userAccess => userAccess.UpdatedBy).Use(user);
=======

                .OnProperty(impersonationContext => impersonationContext.IdentifierColumn)
                    .Use(() => GetRandomStringWithLengthOf(10))

                .OnProperty(impersonationContext => impersonationContext.CreatedBy).Use(user)
                .OnProperty(impersonationContext => impersonationContext.UpdatedBy).Use(user);
>>>>>>> MAJOR CODE RUB:  Rename DelegatedAccess to ImpersonationContext, moved ODS and PDS to ReIdentificationStorageBroker, Unified migrations:ISL.ReIdentification.Core.Tests.Unit/Services/Foundations/ImpersonationContexts/ImpersonationContextTests.cs

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
