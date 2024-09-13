﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class DelegatedAccessServiceException : Xeption
    {
        public DelegatedAccessServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
