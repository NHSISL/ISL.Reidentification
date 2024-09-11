// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class DelegatedAccessDependencyValidationException : Xeption
    {
        public DelegatedAccessDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
