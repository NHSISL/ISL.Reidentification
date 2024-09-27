// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.ReIdentification.Core.Models.Brokers.NECS
{
    public class NECSConfiguration
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public int ApiBulkRequestLimit { get; set; }
    }
}
