using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class LockedLookupException : Xeption
    {
        public LockedLookupException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}