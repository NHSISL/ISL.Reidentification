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
                .OnProperty(lookup => lookup.CreatedDate).Use(now)
                .OnProperty(lookup => lookup.CreatedBy).Use(user)
                .OnProperty(lookup => lookup.UpdatedDate).Use(now)
                .OnProperty(lookup => lookup.UpdatedBy).Use(user);

            return filler;
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
