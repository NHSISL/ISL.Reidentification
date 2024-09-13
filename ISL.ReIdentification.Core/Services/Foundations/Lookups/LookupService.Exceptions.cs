using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.Lookups
{
    public partial class LookupService
    {
        private delegate ValueTask<Lookup> ReturningLookupFunction();
        private delegate IQueryable<Lookup> ReturningLookupsFunction();

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
            catch (NotFoundLookupException notFoundLookupException)
            {
                throw CreateAndLogValidationException(notFoundLookupException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsLookupException =
                    new AlreadyExistsLookupException(
                        message: "Lookup with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsLookupException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidLookupReferenceException =
                    new InvalidLookupReferenceException(
                        message: "Invalid lookup reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidLookupReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedLookupException = 
                    new LockedLookupException(
                        message: "Locked lookup record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedLookupException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedLookupStorageException =
                    new FailedLookupStorageException(
                        message: "Failed lookup storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedLookupStorageException);
            }
            catch (Exception exception)
            {
                var failedLookupServiceException =
                    new FailedLookupServiceException(
                        message: "Failed lookup service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedLookupServiceException);
            }
        }

        private IQueryable<Lookup> TryCatch(ReturningLookupsFunction returningLookupsFunction)
        {
            try
            {
                return returningLookupsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedLookupStorageException =
                    new FailedLookupStorageException(
                        message: "Failed lookup storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedLookupStorageException);
            }
            catch (Exception exception)
            {
                var failedLookupServiceException =
                    new FailedLookupServiceException(
                        message: "Failed lookup service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedLookupServiceException);
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

        private LookupDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var lookupDependencyValidationException =
                new LookupDependencyValidationException(
                    message: "Lookup dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(lookupDependencyValidationException);

            return lookupDependencyValidationException;
        }

        private LookupDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var lookupDependencyException = 
                new LookupDependencyException(
                    message: "Lookup dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogError(lookupDependencyException);

            return lookupDependencyException;
        }

        private LookupServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var lookupServiceException = 
                new LookupServiceException(
                    message: "Lookup service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(lookupServiceException);

            return lookupServiceException;
        }
    }
}