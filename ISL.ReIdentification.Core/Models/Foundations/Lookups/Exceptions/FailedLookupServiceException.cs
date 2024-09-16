using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class FailedLookupServiceException : Xeption
    {
        public FailedLookupServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}