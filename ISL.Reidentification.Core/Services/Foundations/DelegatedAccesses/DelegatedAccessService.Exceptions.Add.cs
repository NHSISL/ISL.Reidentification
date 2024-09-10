// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Xeptions;

namespace ISL.Reidentification.Core.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessService
    {
        private delegate ValueTask<DelegatedAccess> ReturningDelegatedAccessFunction();
        private async ValueTask<DelegatedAccess> TryCatch(ReturningDelegatedAccessFunction returningDelegatedAccessFunction)
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
    }
}
