// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions;
using ISL.ReIdentification.Core.Models.Orchestrations.Identifications.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Identifications
{
    public partial class IdentificationOrchestrationService : IIdentificationOrchestrationService
    {
        private delegate ValueTask<IdentificationRequest> ReturningIdentificationRequestFunction();

        private async ValueTask<IdentificationRequest> TryCatch(
            ReturningIdentificationRequestFunction returningIdentificationRequestFunction)
        {
            try
            {
                return await returningIdentificationRequestFunction();
            }
            catch (AccessAuditValidationException
                accessAuditValidationException)
            {
                throw await CreateAndLogDependencyValidationException(
                    accessAuditValidationException);
            }
            catch (AccessAuditDependencyValidationException
                accessAuditDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(
                    accessAuditDependencyValidationException);
            }
            catch (ReIdentificationValidationException
                reIdentificationValidationException)
            {
                throw await CreateAndLogDependencyValidationException(
                    reIdentificationValidationException);
            }
            catch (ReIdentificationDependencyValidationException
                reIdentificationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationException(
                    reIdentificationDependencyValidationException);
            }
        }

        private async ValueTask<IdentificationOrchestrationDependencyValidationException>
            CreateAndLogDependencyValidationException(Xeption exception)
        {
            var identificationOrchestrationDependencyValidationException =
                new IdentificationOrchestrationDependencyValidationException(
                    message: "Identification orchestration dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(identificationOrchestrationDependencyValidationException);

            return identificationOrchestrationDependencyValidationException;
        }
    }
}
