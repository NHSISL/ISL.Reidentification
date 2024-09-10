// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    internal class FailedOperationDelegatedAccessException : Xeption
    {
        private string message;
        private DbUpdateException innerException;

        public FailedOperationDelegatedAccessException(string message, DbUpdateException innerException)
            : base(message, innerException)
        { }
    }
}
