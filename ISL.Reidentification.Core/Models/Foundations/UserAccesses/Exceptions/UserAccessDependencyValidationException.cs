// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions
{
    public class UserAccessDependencyValidationException : Xeption
    {
        public UserAccessDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
