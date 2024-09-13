using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
            catch (InvalidLookupException invalidLookupException)
            {
                throw CreateAndLogValidationException(invalidLookupException);
            }
            catch (SqlException sqlException)
            {
                var failedLookupStorageException =
                    new FailedLookupStorageException(
                        message: "Failed lookup storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedLookupStorageException);
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

        private LookupDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var lookupDependencyException = 
                new LookupDependencyException(
                    message: "Lookup dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(lookupDependencyException);

            return lookupDependencyException;
        }
    }
}