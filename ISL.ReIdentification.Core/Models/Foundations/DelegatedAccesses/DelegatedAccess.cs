// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses
{
    public class DelegatedAccess : IKey, IAudit
    {
        public Guid Id { get; set; }
        public string RequesterFirstName { get; set; } = string.Empty;
        public string RequesterLastName { get; set; } = string.Empty;
        public string RequesterEmail { get; set; } = string.Empty;
        public string RecipientFirstName { get; set; } = string.Empty;
        public string RecipientLastName { get; set; } = string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
        public bool IsSystemAccess { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ReasonMessage { get; set; } = string.Empty;
        public bool? IsApproved { get; set; } = null;
        public byte[]? Data { get; set; }
        public string IdentifierColumn { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
    }
}