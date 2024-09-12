// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions
{
    public class AlreadyExistsDelegatedAccessException : Xeption
    {
        public AlreadyExistsDelegatedAccessException(string message, Exception innerException, IDictionary data)
                : base(message, innerException, data)
        { }
    }
}
