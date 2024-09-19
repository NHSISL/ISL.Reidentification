﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions
{
    public class OdsDataValidationException : Xeption
    {
        public OdsDataValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
