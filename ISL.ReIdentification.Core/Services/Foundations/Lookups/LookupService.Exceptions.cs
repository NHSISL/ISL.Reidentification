using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public partial class LookupService
    {
        private delegate ValueTask<Lookup> ReturningLookupFunction();

        private async ValueTask<Lookup> TryCatch(ReturningLookupFunction returningLookupFunction)
        {
            try
            {
                return await returningLookupFunction();
            }
            catch (NullLookupException nullLookupException)
            {
                throw CreateAndLogValidationException(nullLookupException);
            }
        }

        private LookupValidationException CreateAndLogValidationException(Xeption exception)
        {
            var lookupValidationException =
                new LookupValidationException(
                    message: "Lookup validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(lookupValidationException);

            return lookupValidationException;
        }
    }
}