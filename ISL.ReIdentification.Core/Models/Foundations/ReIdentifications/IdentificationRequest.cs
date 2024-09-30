// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ISL.ReIdentification.Core.Models.Foundations.ReIdentifications
{
    public class IdentificationRequest
    {
        public Guid Id { get; set; }
        public List<IdentificationItem> IdentificationItems { get; set; }
        public string UserIdentifier { get; set; }
        public string Purpose { get; set; }
        public string Organisation { get; set; }
        public string Reason { get; set; }
    }
}
