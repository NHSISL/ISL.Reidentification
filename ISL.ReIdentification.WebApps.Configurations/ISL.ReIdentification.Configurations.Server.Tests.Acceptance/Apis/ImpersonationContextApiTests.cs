﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Brokers;
using ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.ImpersonationContexts;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Apis
{
    [Collection(nameof(ApiTestCollection))]
    public partial class ImpersonationContextsApiTests
    {
        private readonly ApiBroker apiBroker;

        public ImpersonationContextsApiTests(ApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        private static ImpersonationContext CreateRandomImpersonationContext() =>
           CreateRandomImpersonationContextFiller().Create();

        private int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GetRandomStringWithLengthOf(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static ImpersonationContext UpdateImpersonationContextWithRandomValues(
            ImpersonationContext inputImpersonationContext)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            var updatedImpersonationContext = CreateRandomImpersonationContext();
            updatedImpersonationContext.Id = inputImpersonationContext.Id;
            updatedImpersonationContext.CreatedDate = inputImpersonationContext.CreatedDate;
            updatedImpersonationContext.CreatedBy = inputImpersonationContext.CreatedBy;
            updatedImpersonationContext.UpdatedDate = now;

            return updatedImpersonationContext;
        }

        private async ValueTask<ImpersonationContext> PostRandomImpersonationContextAsync()
        {
            ImpersonationContext randomImpersonationContext = CreateRandomImpersonationContext();
            await this.apiBroker.PostImpersonationContextAsync(randomImpersonationContext);

            return randomImpersonationContext;
        }

        private async ValueTask<List<ImpersonationContext>> PostRandomImpersonationContextsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomImpersonationContexts = new List<ImpersonationContext>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomImpersonationContexts.Add(await PostRandomImpersonationContextAsync());
            }

            return randomImpersonationContexts;
        }

        private static Filler<ImpersonationContext> CreateRandomImpersonationContextFiller()
        {
            string user = Guid.NewGuid().ToString();
            DateTime now = DateTime.UtcNow;
            var filler = new Filler<ImpersonationContext>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(now)

                .OnProperty(impersonationContext => impersonationContext.RequesterFirstName)
                    .Use(() => GetRandomStringWithLengthOf(255))

                .OnProperty(impersonationContext => impersonationContext.RequesterLastName)
                    .Use(() => GetRandomStringWithLengthOf(255))

                .OnProperty(impersonationContext => impersonationContext.RequesterEmail)
                    .Use(() => GetRandomStringWithLengthOf(320))

                .OnProperty(impersonationContext => impersonationContext.RecipientFirstName)
                    .Use(() => GetRandomStringWithLengthOf(255))

                .OnProperty(impersonationContext => impersonationContext.RecipientLastName)
                    .Use(() => GetRandomStringWithLengthOf(255))

                .OnProperty(impersonationContext => impersonationContext.RecipientEmail)
                    .Use(() => GetRandomStringWithLengthOf(320))

                .OnProperty(impersonationContext => impersonationContext.IdentifierColumn)
                    .Use(() => GetRandomStringWithLengthOf(10))

                .OnProperty(impersonationContext => impersonationContext.CreatedDate).Use(now)
                .OnProperty(impersonationContext => impersonationContext.CreatedBy).Use(user)
                .OnProperty(impersonationContext => impersonationContext.UpdatedDate).Use(now)
                .OnProperty(impersonationContext => impersonationContext.UpdatedBy).Use(user);

            return filler;
        }
    }
}
