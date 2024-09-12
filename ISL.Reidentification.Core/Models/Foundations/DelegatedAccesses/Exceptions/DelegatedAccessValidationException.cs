// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class DelegatedAccessValidationException : Xeption
    {
        public DelegatedAccessValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
