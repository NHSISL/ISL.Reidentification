// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Orchestrations.Identifications;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Identifications
{
    public interface IIdentificationCoordinationService
    {
        ValueTask<List<IdentificationRequest>> ProcessIdentificationRequestsAsync(
            List<IdentificationRequest> identificationRequests);
    }
}
