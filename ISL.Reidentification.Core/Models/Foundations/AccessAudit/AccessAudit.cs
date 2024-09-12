// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace ISL.Reidentification.Core.Models.Foundations.AccessAudit
{
    public class AccessAudit
    {
        public Guid Id { get; set; }
        public string PseudoIdentifier { get; set; }
        public string UserEmail { get; set; }
        public bool HasAccess { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedWhen { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedWhen { get; set; }
    }
}
