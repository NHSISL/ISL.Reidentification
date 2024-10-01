// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Brokers.NECS;
using LHDS.Core.Brokers.NECS;
using Tynamix.ObjectFiller;

namespace ISL.ReIdentification.Core.Tests.Unit.Brokers.Necs
{
    public partial class NecsBrokerTests
    {
        private readonly INECSBroker necsBroker;

        public NecsBrokerTests()
        {
            var necsConfiguration = new NECSConfiguration
            {
                ApiUrl = "https://isl-test-app.azurewebsites.net/",
                ApiKey = "62deca33-2b3f-498b-9aa9-acc986ee4a97",
                ApiMaxBatchSize = 500
            };

            necsBroker = new NECSBroker(necsConfiguration);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static string GetRandomStringWithLength(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();
    }
}
