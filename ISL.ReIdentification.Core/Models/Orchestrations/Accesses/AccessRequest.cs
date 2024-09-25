// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;

namespace ISL.ReIdentification.Core.Models.Orchestrations.Accesses
{
    public class AccessRequest
    {
        public List<IdentificationRequest> IdentificationRequests { get; set; } = new List<IdentificationRequest>();
        public DelegatedAccess DelegatedAccessRequest { get; set; }
    }
}
