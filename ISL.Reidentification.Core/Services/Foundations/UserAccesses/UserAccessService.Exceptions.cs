// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using Xeptions;

namespace ISL.Reidentification.Core.Services.Foundations.UserAccesses
{
    public partial class UserAccessService
    {
        private delegate ValueTask<UserAccess> ReturningUserAccessFunction();
        private async ValueTask<UserAccess> TryCatch(ReturningUserAccessFunction returningUserAccessFunction)
        {
            try
            {
                return await returningUserAccessFunction();
            }
            catch (NullUserAccessException nullUserAccessException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullUserAccessException);
            }
        }

        private async ValueTask<UserAccessValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var userAccessValidationException = new UserAccessValidationException(
                message: "UserAccess validation error occurred, please fix errors and try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(userAccessValidationException);

            return userAccessValidationException;
        }
    }
}
