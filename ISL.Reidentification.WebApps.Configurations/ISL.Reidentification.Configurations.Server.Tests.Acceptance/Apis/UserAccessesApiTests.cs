// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.Lookups;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis
{
    [Collection(nameof(ApiTestCollection))]
    public partial class UserAccessesApiTests
    {
        private readonly ApiBroker apiBroker;

        public UserAccessesApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();


        private static UserAccess CreateRandomUserAccess() =>
            CreateRandomUserAccessFiller().Create();

        private static Filler<UserAccess> CreateRandomUserAccessFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<UserAccess>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)
                .OnType<DateTimeOffset?>().Use(now)
                .OnProperty(userAccess => userAccess.CreatedDate).Use(now)
                .OnProperty(userAccess => userAccess.CreatedBy).Use(user)
                .OnProperty(userAccess => userAccess.UpdatedDate).Use(now)
                .OnProperty(userAccess => userAccess.UpdatedBy).Use(user);

            return filler;
        }

        private static UserAccess UpdateUserAccess(UserAccess inputUserAccess)
        {

            DateTimeOffset now = DateTimeOffset.UtcNow;
            var filler = new Filler<UserAccess>();

            filler.Setup()
                .OnProperty(userAccess => userAccess.Id).Use(inputUserAccess.Id)
                .OnType<DateTimeOffset>().Use(GetRandomDateTimeOffset())
                .OnType<DateTimeOffset?>().Use(GetRandomDateTimeOffset())
                .OnProperty(userAccess => userAccess.CreatedDate).Use(inputUserAccess.CreatedDate)
                .OnProperty(userAccess => userAccess.CreatedBy).Use(inputUserAccess.CreatedBy)
                .OnProperty(userAccess => userAccess.UpdatedDate).Use(now);

            return filler.Create();
        }

        private async ValueTask<List<UserAccess>> PostRandomUserAccesses()
        {
            int randomNumber = GetRandomNumber();
            List<UserAccess> randomUserAccesses = new List<UserAccess>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomUserAccesses.Add(await this.PostRandomUserAccess());
            }

            return randomUserAccesses;
        }

        private async ValueTask<UserAccess> PostRandomUserAccess()
        {
            UserAccess randomUserAccess = CreateRandomUserAccess();
            await this.apiBroker.PostUserAccessAsync(randomUserAccess);

            return randomUserAccess;
        }
    }
}
