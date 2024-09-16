// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.AccessAudits
{
    public partial class AccessAuditService
    {
        private delegate ValueTask<AccessAudit> ReturningAccessAuditFunction();
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
    }
}
