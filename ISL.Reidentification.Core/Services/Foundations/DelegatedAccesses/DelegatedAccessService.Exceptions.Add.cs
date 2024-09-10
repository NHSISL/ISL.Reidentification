// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService
    {
        private delegate ValueTask<DelegatedAccess> ReturningDelegatedAccessFunction();
        private async ValueTask<DelegatedAccess> TryCatch(ReturningDelegatedAccessFunction returningDelegatedAccessFunction)
        {
            try
            {
                return await returningDelegatedAccessFunction();
            }
            catch (NullDelegatedAccessException nullDelegatedAccessException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullDelegatedAccessException);
            }
            catch (InvalidDelegatedAccessException invalidDelegatedAccessException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidDelegatedAccessException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageDelegatedAccessException = new FailedStorageDelegatedAccessException(
                    message: "Failed delegated access storage error occurred, contact support.",
                    innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedStorageDelegatedAccessException);
            }
        }

        private async ValueTask<DelegatedAccessValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var delegatedAccessValidationException = new DelegatedAccessValidationException(
                message: "DelegatedAccess validation error occurred, please fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(delegatedAccessValidationException);

            return delegatedAccessValidationException;
        }

        private async ValueTask<DelegatedAccessDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var userAccessDependencyException = new DelegatedAccessDependencyException(
                message: "DelegatedAccess dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(userAccessDependencyException);

            return userAccessDependencyException;
        }
    }
}
