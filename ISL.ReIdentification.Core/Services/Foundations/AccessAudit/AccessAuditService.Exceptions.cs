﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using ISL.Reidentification.Core.Models.Foundations.AccessAudits.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.AccessAudits
{
    public partial class AccessAuditService
    {
        private delegate ValueTask<AccessAudit> ReturningAccessAuditFunction();
        private delegate ValueTask<IQueryable<AccessAudit>> ReturningAccessAuditsFunction();

        private async ValueTask<IQueryable<AccessAudit>> TryCatch(
            ReturningAccessAuditsFunction returningAccessAuditsFunction)
        {
            try
            {
                return await returningAccessAuditsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStorageAccessAuditException = new FailedStorageAccessAuditException(
                    message: "Failed access audit storage error occurred, contact support.",
                    innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedStorageAccessAuditException);
            }
            catch (Exception exception)
            {
                var failedServiceAccessAuditException =
                    new FailedServiceAccessAuditException(
                        message: "Failed service access audit error occurred, contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedServiceAccessAuditException);
            }
        }
        private async ValueTask<AccessAudit> TryCatch(ReturningAccessAuditFunction returningAccessAuditFunction)
        {
            try
            {
                return await returningAccessAuditFunction();
            }
            catch (NullAccessAuditException nullAccessAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullAccessAuditException);
            }
            catch (InvalidAccessAuditException invalidAccessAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidAccessAuditException);
            }
            catch (NotFoundAccessAuditException notFoundAccessAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundAccessAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageAccessAuditException = new FailedStorageAccessAuditException(
                    message: "Failed access audit storage error occurred, contact support.",
                    innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedStorageAccessAuditException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAccessAuditException =
                    new AlreadyExistsAccessAuditException(
                        message: "Access audit already exists error occurred.",
                        innerException: duplicateKeyException,
                        data: duplicateKeyException.Data);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsAccessAuditException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAccessAuditException =
                    new LockedAccessAuditException(
                        message: "Locked access audit record error occurred, please try again.",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedAccessAuditException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedOperationAccessAuditException =
                    new FailedOperationAccessAuditException(
                        message: "Failed operation access audit error occurred, contact support.",
                        innerException: dbUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedOperationAccessAuditException);
            }
            catch (Exception exception)
            {
                var failedServiceAccessAuditException =
                    new FailedServiceAccessAuditException(
                        message: "Failed service access audit error occurred, contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedServiceAccessAuditException);
            }
        }

        private async ValueTask<AccessAuditValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var accessAuditValidationException = new AccessAuditValidationException(
                message: "Access audit validation error occurred, please fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(accessAuditValidationException);

            return accessAuditValidationException;
        }

        private async ValueTask<AccessAuditDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
           Xeption exception)
        {
            var userAccessDependencyException = new AccessAuditDependencyException(
                message: "Access audit dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(userAccessDependencyException);

            return userAccessDependencyException;
        }

        private async ValueTask<AccessAuditDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var userAccessDependencyValidationException = new AccessAuditDependencyValidationException(
                message: "Access audit dependency validation error occurred, fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessDependencyValidationException);

            return userAccessDependencyValidationException;
        }

        private async ValueTask<AccessAuditDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var userAccessDependencyException = new AccessAuditDependencyException(
                message: "Access audit dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessDependencyException);

            return userAccessDependencyException;
        }

        private async ValueTask<AccessAuditServiceException> CreateAndLogServiceExceptionAsync(
           Xeption exception)
        {
            var userAccessServiceException = new AccessAuditServiceException(
                message: "Service error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessServiceException);

            return userAccessServiceException;
        }
    }
}
