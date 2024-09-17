// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public interface IAccessOrchestrationService
    {
        ValueTask<List<IdentificationRequest>> ValidateAccessForIdentificationRequestsAsync(
            List<IdentificationRequest> identificationRequests);
    }
}
