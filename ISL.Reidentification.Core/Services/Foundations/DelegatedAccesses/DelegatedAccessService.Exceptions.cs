// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService
    {
        private delegate ValueTask<DelegatedAccess> ReturningDelegatedAccessFunction();

        private async ValueTask<DelegatedAccess> TryCatch(
            ReturningDelegatedAccessFunction returningDelegatedAccessFunction)
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
            catch (NotFoundDelegatedAccessException notFoundDelegatedAccessException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundDelegatedAccessException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageDelegatedAccessException = new FailedStorageDelegatedAccessException(
                    message: "Failed delegated access storage error occurred, contact support.",
                    innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedStorageDelegatedAccessException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsDelegatedAccessException =
                    new AlreadyExistsDelegatedAccessException(
                        message: "DelegatedAccess already exists error occurred.",
                        innerException: duplicateKeyException,
                        data: duplicateKeyException.Data);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsDelegatedAccessException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedOperationDelegatedAccessException =
                    new FailedOperationDelegatedAccessException(
                        message: "Failed operation delegated access error occurred, contact support.",
                        innerException: dbUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedOperationDelegatedAccessException);
            }
            catch (Exception exception)
            {
                var failedServiceDelegatedAccessException =
                    new FailedServiceDelegatedAccessException(
                        message: "Failed service delegated access error occurred, contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedServiceDelegatedAccessException);
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
            var delegatedAccessDependencyException = new DelegatedAccessDependencyException(
                message: "DelegatedAccess dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(delegatedAccessDependencyException);

            return delegatedAccessDependencyException;
        }

        private async ValueTask<DelegatedAccessDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var delegatedAccessDependencyValidationException = new DelegatedAccessDependencyValidationException(
                message: "DelegatedAccess dependency validation error occurred, fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(delegatedAccessDependencyValidationException);

            return delegatedAccessDependencyValidationException;
        }

        private async ValueTask<DelegatedAccessDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var delegatedAccessDependencyException = new DelegatedAccessDependencyException(
                message: "DelegatedAccess dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(delegatedAccessDependencyException);

            return delegatedAccessDependencyException;
        }

        private async ValueTask<DelegatedAccessServiceException> CreateAndLogServiceExceptionAsync(
           Xeption exception)
        {
            var delegatedAccessServiceException = new DelegatedAccessServiceException(
                message: "Service error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(delegatedAccessServiceException);

            return delegatedAccessServiceException;
        }
    }
}
