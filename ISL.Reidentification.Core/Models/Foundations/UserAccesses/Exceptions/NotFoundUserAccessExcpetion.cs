// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions
{
    public class NotFoundUserAccessException : Xeption
    {
        public NotFoundUserAccessException(string message)
            : base(message)
        { }
    }
}
