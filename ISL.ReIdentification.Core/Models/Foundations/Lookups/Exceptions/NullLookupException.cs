using Xeptions;

namespace ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions
{
    public class NullLookupException : Xeption
    {
        public NullLookupException(string message)
            : base(message)
        { }
    }
}