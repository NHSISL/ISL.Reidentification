// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class FailedStorageDelegatedAccessException : Xeption
    {
        public FailedStorageDelegatedAccessException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
