// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class FailedServiceDelegatedAccessException : Xeption
    {
        public FailedServiceDelegatedAccessException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
