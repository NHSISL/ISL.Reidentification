﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.DelegatedAccesses;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis
{
    [Collection(nameof(ApiTestCollection))]
    public partial class DelegatedAccessesApiTests
    {
        private readonly ApiBroker apiBroker;

        public DelegatedAccessesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static DelegatedAccess CreateRandomDelegatedAccess() =>
           CreateRandomDelegatedAccessFiller().Create();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomStringWithLengthOf(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static DelegatedAccess UpdateDelegatedAccessWithRandomValues(DelegatedAccess inputDelegatedAccess)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<DelegatedAccess>();

            filler.Setup()
                .OnProperty(delegatedAccess => delegatedAccess.Id).Use(inputDelegatedAccess.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime())
                .OnProperty(delegatedAccess => delegatedAccess.CreatedDate).Use(inputDelegatedAccess.CreatedDate)
                .OnProperty(delegatedAccess => delegatedAccess.CreatedBy).Use(inputDelegatedAccess.CreatedBy)
                .OnProperty(delegatedAccess => delegatedAccess.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<DelegatedAccess> PostRandomDelegatedAccessAsync()
        {
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            await this.apiBroker.PostDelegatedAccessAsync(randomDelegatedAccess);

            return randomDelegatedAccess;
        }

        private static Filler<DelegatedAccess> CreateRandomDelegatedAccessFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<DelegatedAccess>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnProperty(delegatedAccess => delegatedAccess.RequesterEmail)
                    .Use(() => GetRandomStringWithLengthOf(320))

                .OnProperty(delegatedAccess => delegatedAccess.RecipientEmail)
                    .Use(() => GetRandomStringWithLengthOf(320))

                .OnProperty(delegatedAccess => delegatedAccess.IdentifierColumn)
                    .Use(() => GetRandomStringWithLengthOf(10))

                .OnProperty(delegatedAccess => delegatedAccess.CreatedDate).Use(now)
                .OnProperty(delegatedAccess => delegatedAccess.CreatedBy).Use(user)
                .OnProperty(delegatedAccess => delegatedAccess.UpdatedDate).Use(now)
                .OnProperty(delegatedAccess => delegatedAccess.UpdatedBy).Use(user);

            return filler;
        }
    }
}
