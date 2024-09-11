// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions
{
    internal class FailedOperationUserAccessException : Xeption
    {
        public FailedOperationUserAccessException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
