// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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

        private static string GetRandomStringWithLengthOf(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

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
