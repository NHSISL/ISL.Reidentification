﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace ISL.ReIdentification.Core.Services.Foundations.Ods
{
    public partial class OdsService : IOdsService
    {
        private delegate ValueTask<IQueryable<OdsData>> ReturningOdsDatasFunction();

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

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedStorageOdsDataException);
            }
        }

        private async ValueTask<OdsDataDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var odsDataDependencyException = new OdsDataDependencyException(
                message: "OdsData dependency error occurred, contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(odsDataDependencyException);

            return odsDataDependencyException;
        }
    }
}
