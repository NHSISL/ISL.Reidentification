// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests;

namespace ISL.ReIdentification.Core.Services.Foundations.CsvIdentificationRequests
{
    public interface ICsvIdentificationRequestService
    {
        ValueTask<CsvIdentificationRequest> AddCsvIdentificationRequestAsync(CsvIdentificationRequest impersonationContext);
        ValueTask<CsvIdentificationRequest> RetrieveCsvIdentificationRequestByIdAsync(Guid impersonationContextId);
        ValueTask<IQueryable<CsvIdentificationRequest>> RetrieveAllCsvIdentificationRequestsAsync();
        ValueTask<CsvIdentificationRequest> ModifyCsvIdentificationRequestAsync(CsvIdentificationRequest impersonationContext);
        ValueTask<CsvIdentificationRequest> RemoveCsvIdentificationRequestByIdAsync(Guid impersonationContextId);
    }
}
