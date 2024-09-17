// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.ReIdentification.Core.Models.Foundations.ReIdentifications
{
    public class IdentificationRequest
    {
        public int RowNumber { get; set; }
        public string Identifier { get; set; }
        public string UserEmail { get; set; }
        public bool HasAccess { get; set; }
        public bool IsReidentified { get; set; }
    }
}
