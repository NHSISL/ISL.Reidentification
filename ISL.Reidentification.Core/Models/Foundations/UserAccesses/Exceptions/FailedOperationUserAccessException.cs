// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions
{
    internal class FailedOperationUserAccessException : Xeption
    {
        private string message;
        private DbUpdateException innerException;

        public FailedOperationUserAccessException(string message, DbUpdateException innerException)
            : base(message, innerException)
        { }
    }
}
