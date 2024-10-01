// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService
    {
        private delegate ValueTask<List<string>> ReturningOrganisationsFunction();

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

        private AccessValidationOrchestrationException CreateAndLogValidationException(Xeption exception)
        {
            var accessValidationException =
                new AccessValidationOrchestrationException(
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
    }
}
