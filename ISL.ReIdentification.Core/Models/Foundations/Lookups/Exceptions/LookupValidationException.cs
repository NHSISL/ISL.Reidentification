using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class LookupValidationException : Xeption
    {
        public LookupValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}