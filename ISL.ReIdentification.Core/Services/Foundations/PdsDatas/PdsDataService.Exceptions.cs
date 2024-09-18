// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public partial class PdsDataService
    {
        private delegate ValueTask<PdsData> ReturningPdsDataFunction();

        private async ValueTask<PdsData> TryCatch(ReturningPdsDataFunction returningPdsDataFunction)
        {
            try
            {
                return await returningPdsDataFunction();
            }
            catch (InvalidPdsDataException invalidPdsDataException)
            {
                throw CreateAndLogValidationException(invalidPdsDataException);
            }
        }

        private PdsDataValidationException CreateAndLogValidationException(Xeption exception)
        {
            var pdsDataValidationException =
                new PdsDataValidationException(
                    message: "PdsData validation error occurred, please fix errors and try again.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(pdsDataValidationException);

            return pdsDataValidationException;
        }
    }
}
