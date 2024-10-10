// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts
{
    public class ImpersonationContext : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string RequesterFirstName { get; set; } = string.Empty;
        public string RequesterLastName { get; set; } = string.Empty;
        public string RequesterEmail { get; set; } = string.Empty;
        public string RecipientFirstName { get; set; } = string.Empty;
        public string RecipientLastName { get; set; } = string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string Organisation { get; set; } = string.Empty;
        public bool? IsApproved { get; set; } = null;
        public string IdentifierColumn { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
    }
}