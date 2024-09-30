﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ISL.ReIdentification.Core.Models.Brokers.NECS.Requests
{
    public class NecsReIdentificationResponse
    {
        [JsonProperty("uniqueRequestId")]
        public Guid UniqueRequestId { get; set; }

        [JsonProperty("results")]
        public List<NecsReidentifiedItem> Results { get; set; }

        [JsonProperty("elapsedTime")]
        public int ElapsedTime { get; set; }

        [JsonProperty("processedCount")]
        public string ProcessedCount { get; set; }
    }
}
