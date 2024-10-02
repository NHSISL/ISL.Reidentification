// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Coordinations.Identifications.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications.Exceptions;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses.Exceptions;
using ISL.ReIdentification.Core.Models.Orchestrations.Identifications.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Identifications
{
    public partial class IdentificationCoordinationService : IIdentificationCoordinationService
    {
        private delegate ValueTask<AccessRequest> ReturningAccessRequestFunction();

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
            catch (NullIdentificationRequestException nullIdentificationRequestException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullIdentificationRequestException);
            }
            catch (IdentificationOrchestrationValidationException identificationOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    identificationOrchestrationValidationException);
            }
            catch (IdentificationOrchestrationDependencyValidationException identificationOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    identificationOrchestrationDependencyValidationException);
            }
            catch (AccessOrchestrationValidationException accessOrchestrationValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    accessOrchestrationValidationException);
            }
            catch (AccessOrchestrationDependencyValidationException accessOrchestrationDependencyValidationException)
            {
                throw await CreateAndLogDependencyValidationExceptionAsync(
                    accessOrchestrationDependencyValidationException);
            }
        }

        private async ValueTask<IdentificationCoordinationValidationException>
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var identificationCoordinationValidationException =
                new IdentificationCoordinationValidationException(
                    message: "Identification coordination validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(identificationCoordinationValidationException);

            return identificationCoordinationValidationException;
        }

        private async ValueTask<IdentificationCoordinationDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var identificationCoordinationDependencyValidationException =
                new IdentificationCoordinationDependencyValidationException(
                    message: "Identification coordination dependency validation error occurred, " +
                        "fix the errors and try again.",
                    innerException: exception.InnerException as Xeption);

            await this.loggingBroker.LogErrorAsync(identificationCoordinationDependencyValidationException);

            return identificationCoordinationDependencyValidationException;
        }
    }
}
