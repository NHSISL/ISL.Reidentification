using System;
using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class NotFoundLookupException : Xeption
    {
        public NotFoundLookupException(Guid lookupId)
            : base(message: $"Couldn't find lookup with lookupId: {lookupId}.")
        { }
    }
}