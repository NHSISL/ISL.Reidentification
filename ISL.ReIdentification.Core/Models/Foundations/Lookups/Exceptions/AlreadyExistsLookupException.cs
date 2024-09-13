using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class AlreadyExistsLookupException : Xeption
    {
        public AlreadyExistsLookupException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}