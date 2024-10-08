// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;

namespace ISL.ReIdentification.Core.Models.Foundations.OdsDatas
{
    public class OdsData : IKey
    {
        public Guid Id { get; set; }
        public string OrganisationCode_Root { get; set; }
        public string OrganisationPrimaryRole_Root { get; set; }
        public string OrganisationCode_Parent { get; set; }
        public string OrganisationPrimaryRole_Parent { get; set; }
        public DateTimeOffset RelationshipStartDate { get; set; }
        public DateTimeOffset RelationshipEndDate { get; set; }
        public string Path { get; set; }
        public int Depth { get; set; }
        public PdsData PdsData { get; set; }
    }
}
