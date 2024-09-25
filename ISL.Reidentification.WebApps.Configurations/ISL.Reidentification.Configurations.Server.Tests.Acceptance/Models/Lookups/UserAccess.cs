// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.Lookups
{
    public class UserAccess
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string OrgCode { get; set; } = string.Empty;
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
