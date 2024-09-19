// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.OdsDatas
{
    public partial class OdsDataService : IOdsDataService
    {
        private delegate ValueTask<IQueryable<OdsData>> ReturningOdsDatasFunction();
        private delegate ValueTask<OdsData> ReturningOdsDataFunction();

        private async ValueTask<IQueryable<OdsData>> TryCatch(ReturningOdsDatasFunction returningOdsDatasFunction)
        {
            try
            {
                return await returningOdsDatasFunction();
            }
            catch (SqlException sqlException)
            {
                var failedStorageOdsDataException = new FailedStorageOdsDataException(
                    message: "Failed ODS data storage error occurred, contact support.",
                    innerException: sqlException);

                throw CreateAndLogCriticalDependencyExceptionAsync(failedStorageOdsDataException);
            }
            catch (Exception exception)
            {
                var failedServiceodsDataException = new FailedServiceOdsDataException(
                    message: "Failed service ODS data error occurred, contact support.",
                    innerException: exception);

                throw CreateAndLogServiceExceptionAsync(failedServiceodsDataException);
            }
        }

        private async ValueTask<OdsData> TryCatch(ReturningOdsDataFunction returningOdsDataFunction)
        {
            try
            {
                return await returningOdsDataFunction();
            }
            catch (InvalidOdsDataException invalidOdsDataException)
            {
                throw CreateAndLogValidationException(invalidOdsDataException);
            }
            catch (NotFoundOdsDataException notFoundOdsDataException)
            {
                throw CreateAndLogValidationException(notFoundOdsDataException);
            }
            catch (SqlException sqlException)
            {
                var failedStorageOdsDataException = new FailedStorageOdsDataException(
                    message: "Failed ODS data storage error occurred, contact support.",
                    innerException: sqlException);

                throw CreateAndLogCriticalDependencyExceptionAsync(failedStorageOdsDataException);
            }
        }

        private OdsDataDependencyException CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var odsDataDependencyException = new OdsDataDependencyException(
                message: "OdsData dependency error occurred, contact support.",
                innerException: exception);

            this.loggingBroker.LogCriticalAsync(odsDataDependencyException);

            return odsDataDependencyException;
        }

        private OdsDataServiceException CreateAndLogServiceExceptionAsync(
            FailedServiceOdsDataException failedServiceOdsDataException)
        {
            var odsDataServiceException = new OdsDataServiceException(
                message: "Service error occurred, contact support.",
                innerException: failedServiceOdsDataException);

            this.loggingBroker.LogErrorAsync(odsDataServiceException);

            return odsDataServiceException;
        }

        private OdsDataValidationException CreateAndLogValidationException(Xeption exception)
        {
            var odsDataValidationException = new OdsDataValidationException(
                message: "OdsData validation error occurred, please fix errors and try again.",
                innerException: exception);

            this.loggingBroker.LogErrorAsync(odsDataValidationException);

            return odsDataValidationException;
        }
    }
}
