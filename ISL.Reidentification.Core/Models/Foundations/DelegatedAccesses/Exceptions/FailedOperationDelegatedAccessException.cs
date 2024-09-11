// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class FailedOperationDelegatedAccessException : Xeption
    {
        public FailedOperationDelegatedAccessException(string message, DbUpdateException innerException)
            : base(message, innerException)
        { }
    }
}
