// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Brokers.NECS.Requests;

namespace ISL.ReIdentification.Core.Tests.Unit.Brokers.Necs
{
    public partial class NecsBrokerTests
    {
        [Fact]
        private async Task ShouldProcessReidentificationRequest()
        {
            NecsReidentificationRequest necsReidentificationRequest = new NecsReidentificationRequest
            {
                RequestId = Guid.NewGuid(),
                UserIdentifier = GetRandomString(),
                Purpose = GetRandomString(),
                Organisation = GetRandomString(),
                Reason = GetRandomString(),
                PseudonymisedNumbers = new List<NecsPseudonymisedItem>
                {
                    new NecsPseudonymisedItem
                    {
                        RowNumber = GetRandomNumber().ToString(),
                        Psuedo = GetRandomStringWithLength(10)
                    }
                }
            };

            var result = await this.necsBroker.ReIdAsync(necsReidentificationRequest);
        }

        [Fact]
        private async Task ShouldValidationExceptionOnProcessReidentificationRequest()
        {
            NecsReidentificationRequest necsReidentificationRequest = new NecsReidentificationRequest
            {
                RequestId = Guid.NewGuid(),
                UserIdentifier = GetRandomString(),
                Purpose = GetRandomString(),
                Organisation = GetRandomString(),
                Reason = GetRandomString(),
                PseudonymisedNumbers = new List<NecsPseudonymisedItem>
                {
                    new NecsPseudonymisedItem
                    {
                        RowNumber = 1.ToString(),
                    },
                    new NecsPseudonymisedItem
                    {
                        RowNumber = 1.ToString(),
                        Psuedo = GetRandomStringWithLength(10)
                    },
                }
            };

            var result = await this.necsBroker.ReIdAsync(necsReidentificationRequest);
        }
    }
}
