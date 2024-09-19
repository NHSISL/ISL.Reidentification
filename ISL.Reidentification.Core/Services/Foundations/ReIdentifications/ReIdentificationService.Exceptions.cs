// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationService
    {
        private delegate ValueTask<IdentificationRequest> ReturningIdentificationRequestFunction();

        private async ValueTask<IdentificationRequest> TryCatch(
            ReturningIdentificationRequestFunction returningIdentificationRequestFunction)
        {
            try
            {
                return await returningIdentificationRequestFunction();
            }
            catch (NullIdentificationRequestException nullIdentificationRequestException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullIdentificationRequestException);
            }
            catch (InvalidIdentificationRequestException invalidIdentificationRequestException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidIdentificationRequestException);
            }
            catch (Exception exception)
            {
                var failedServiceIdentificationRequestException =
                    new FailedServiceReIdentificationException(
                        message: "Failed service re identification error occurred, contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedServiceIdentificationRequestException);
            }
        }

        private async ValueTask<IdentificationRequestValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var accessAuditValidationException = new IdentificationRequestValidationException(
                message: "Re identification validation error occurred, please fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(accessAuditValidationException);

            return accessAuditValidationException;
        }


        private async ValueTask<ReIdentificationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var userAccessDependencyValidationException = new ReIdentificationDependencyValidationException(
                message: "Re identification dependency validation error occurred, fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessDependencyValidationException);

            return userAccessDependencyValidationException;
        }

        private async ValueTask<ReIdentificationDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var userAccessDependencyException = new ReIdentificationDependencyException(
                message: "Re identification dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessDependencyException);

            return userAccessDependencyException;
        }

        private async ValueTask<ReIdentificationServiceException> CreateAndLogServiceExceptionAsync(
           Xeption exception)
        {
            var userAccessServiceException = new ReIdentificationServiceException(
                message: "Service error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessServiceException);

            return userAccessServiceException;
        }
    }
}
