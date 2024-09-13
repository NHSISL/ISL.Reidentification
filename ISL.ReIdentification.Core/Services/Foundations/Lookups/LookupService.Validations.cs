using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public partial class LookupService
    {
        private void ValidateLookupOnAdd(Lookup lookup)
        {
            ValidateLookupIsNotNull(lookup);
        }

        private static void ValidateLookupIsNotNull(Lookup lookup)
        {
            if (lookup is null)
            {
                throw new NullLookupException(message: "Lookup is null.");
            }
        }
    }
}