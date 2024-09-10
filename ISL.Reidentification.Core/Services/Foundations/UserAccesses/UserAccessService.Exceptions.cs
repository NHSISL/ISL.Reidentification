// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace ISL.Reidentification.Core.Services.Foundations.UserAccesses
{
    public partial class UserAccessService
    {
        private delegate ValueTask<UserAccess> ReturningUserAccessFunction();
        private async ValueTask<UserAccess> TryCatch(ReturningUserAccessFunction returningUserAccessFunction)
        {
            try
            {
                return await returningUserAccessFunction();
            }
            catch (NullUserAccessException nullUserAccessException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullUserAccessException);
            }
            catch (InvalidUserAccessException invalidUserAccessException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidUserAccessException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageUserAccessException = new FailedStorageUserAccessException(
                    message: "Failed user access storage error occurred, contact support.",
                    innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedStorageUserAccessException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsUserAccessException =
                    new AlreadyExistsUserAccessException(
                        message: "UserAccess already exists error occurred.",
                        innerException: duplicateKeyException,
                        data: duplicateKeyException.Data);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsUserAccessException);
            }
        }

        private async ValueTask<UserAccessValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var userAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessValidationException);

            return userAccessValidationException;
        }

        private async ValueTask<UserAccessDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var userAccessDependencyException = new UserAccessDependencyException(
                message: "UserAccess dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(userAccessDependencyException);

            return userAccessDependencyException;
        }

        private async ValueTask<UserAccessDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(
            Xeption exception)
        {
            var userAccessDependencyValidationException = new UserAccessDependencyValidationException(
                message: "UserAccess dependency validation error occurred, fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessDependencyValidationException);

            return userAccessDependencyValidationException;
        }
    }
}
