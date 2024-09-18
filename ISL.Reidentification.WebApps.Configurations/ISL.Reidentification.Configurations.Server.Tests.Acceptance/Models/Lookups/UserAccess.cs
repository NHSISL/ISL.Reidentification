// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace ISL.ReIdentification.Configurations.Server.Tests.Acceptance.Models.Lookups
{
    public class UserAccess
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; }
        public string RecipientEmail { get; set; }
        public string OrgCode { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
