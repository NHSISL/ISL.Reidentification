// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.PdsDatas
{
    public partial class PdsDataService
    {
        private delegate ValueTask<IQueryable<PdsData>> ReturningPdsDatasFunction();

        private async ValueTask<IQueryable<PdsData>> TryCatch(ReturningPdsDatasFunction returningPdsDatasFunction)
        {
            try
            {
                return await returningPdsDatasFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStoragePdsDataException =
                    new FailedStoragePdsDataException(
                        message: "Failed pds data storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedStoragePdsDataException);
            }
            catch (Exception exception)
            {
                var failedPdsDataServiceException =
                    new FailedServicePdsDataException(
                        message: "Failed pds data service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedPdsDataServiceException);
            }
        }

        private PdsDataDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var pdsDataDependencyException =
                new PdsDataDependencyException(
                    message: "PdsData dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCriticalAsync(pdsDataDependencyException);

            return pdsDataDependencyException;
        }

        private PdsDataServiceException CreateAndLogServiceException(Xeption exception)
        {
            var pdsDataServiceException =
                new PdsDataServiceException(
                    message: "Service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogErrorAsync(pdsDataServiceException);

            return pdsDataServiceException;
        }
    }
}
