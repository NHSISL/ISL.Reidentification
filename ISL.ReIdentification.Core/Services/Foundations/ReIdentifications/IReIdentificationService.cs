// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Orchestrations.Identifications;

namespace ISL.ReIdentification.Core.Services.Foundations.ReIdentifications
{
    public interface IReIdentificationService
    {
        ValueTask<List<IdentificationRequest>> ProcessReidentificationRequests(
            List<IdentificationRequest> identificationRequests);
    }
}
