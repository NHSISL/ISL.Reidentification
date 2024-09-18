// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions
{
    public class FailedServiceReIdentificationException : Xeption
    {
        public FailedServiceReIdentificationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
