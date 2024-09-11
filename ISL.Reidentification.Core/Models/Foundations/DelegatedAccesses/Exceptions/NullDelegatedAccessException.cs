﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class NullDelegatedAccessException : Xeption
    {
        public NullDelegatedAccessException(string message)
            : base(message)
        { }
    }
}
