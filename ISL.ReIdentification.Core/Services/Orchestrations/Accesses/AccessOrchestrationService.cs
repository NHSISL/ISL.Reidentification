// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;

namespace ISL.ReIdentification.Core.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationService : IAccessOrchestrationService
    {
        public async ValueTask<List<IdentificationRequest>> ValidateAccessForIdentificationRequestsAsync(
            List<IdentificationRequest> identificationRequests) =>
            throw new NotImplementedException();
    }
}
