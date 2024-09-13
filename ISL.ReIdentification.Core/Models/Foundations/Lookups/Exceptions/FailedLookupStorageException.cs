using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class FailedLookupStorageException : Xeption
    {
        public FailedLookupStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}