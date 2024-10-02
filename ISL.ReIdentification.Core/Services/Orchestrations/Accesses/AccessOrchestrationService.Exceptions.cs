// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses.Exceptions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService
    {
        private delegate ValueTask<List<string>> ReturningOrganisationsFunction();
        private delegate ValueTask<bool> ReturningBooleanFunction();
        private delegate ValueTask<AccessRequest> ReturningAccessRequestFunction();

        private async ValueTask<List<string>> TryCatch(ReturningOrganisationsFunction returningOrganisationsFunction)
        {
            try
            {
                return await returningOrganisationsFunction();
            }
            catch (InvalidArgumentAccessOrchestrationException invalidArgumentAccessOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAccessOrchestrationException);
            }
            catch (Exception exception)
            {
                var failedServiceAccessOrchestrationException =
                    new FailedServiceAccessOrchestrationException(
                        message: "Failed service access orchestration error occurred, contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedServiceAccessOrchestrationException);
            }
        }

        private async ValueTask<bool> TryCatch(ReturningBooleanFunction returningBooleanFunction)
        {
            try
            {
                return await returningBooleanFunction();
            }
            catch (InvalidArgumentAccessOrchestrationException invalidArgumentAccessOrchestrationException)
            {
                throw CreateAndLogValidationException(invalidArgumentAccessOrchestrationException);
            }
            catch (Exception exception)
            {
                var failedServiceAccessOrchestrationException =
                    new FailedServiceAccessOrchestrationException(
                        message: "Failed service access orchestration error occurred, contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedServiceAccessOrchestrationException);
            }
        }

        private async ValueTask<AccessRequest> TryCatch(ReturningAccessRequestFunction returningAccessRequestFunction)
        {
            try
            {
                return await returningAccessRequestFunction();
            }
            catch (NullAccessRequestException nullAccessRequestException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullAccessRequestException);
            }
            catch (UserAccessValidationException userAccessValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    userAccessValidationException);
            }
            catch (UserAccessDependencyValidationException userAccessDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    userAccessDependencyValidationException);
            }
            catch (PdsDataValidationException pdsDataValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    pdsDataValidationException);
            }
        }

        private AccessOrchestrationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var accessValidationException =
                new AccessOrchestrationValidationException(
                    message: "Access orchestration validation error occurred, please fix errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(accessValidationException);

            return accessValidationException;
        }

        private async ValueTask<AccessOrchestrationServiceException> CreateAndLogServiceExceptionAsync(
           Xeption exception)
        {
            var accessOrchestrationServiceException = new AccessOrchestrationServiceException(
                message: "Service error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(accessOrchestrationServiceException);

            return accessOrchestrationServiceException;
        }

        private async ValueTask<AccessOrchestrationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var accessOrchestrationValidationException =
                new AccessOrchestrationValidationException(
                    message: "Access orchestration validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(accessOrchestrationValidationException);

            return accessOrchestrationValidationException;
        }

        private async ValueTask<AccessOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var accessOrchestrationDependencyValidationException =
                new AccessOrchestrationDependencyValidationException(
                    message: "Access orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(accessOrchestrationDependencyValidationException);

            return accessOrchestrationDependencyValidationException;
        }
    }
}
