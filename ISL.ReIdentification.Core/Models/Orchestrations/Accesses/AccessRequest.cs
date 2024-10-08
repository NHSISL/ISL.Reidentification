// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;

namespace ISL.ReIdentification.Core.Models.Orchestrations.Accesses
{
    public class AccessRequest
    {
        public IdentificationRequest IdentificationRequest { get; set; }
        public DelegatedAccess DelegatedAccessRequest { get; set; }
    }
}
