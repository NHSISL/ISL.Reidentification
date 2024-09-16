using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class InvalidLookupReferenceException : Xeption
    {
        public InvalidLookupReferenceException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}