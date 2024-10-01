// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
    }
}
