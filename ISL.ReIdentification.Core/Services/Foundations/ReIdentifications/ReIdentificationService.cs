// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Orchestrations.Identifications;

namespace ISL.ReIdentification.Core.Services.Foundations.ReIdentifications
{
    public partial class ReIdentificationService : IReIdentificationService
    {
        public async ValueTask<List<IdentificationRequest>> ProcessReidentificationRequests(
            List<IdentificationRequest> identificationRequests) =>
            throw new NotImplementedException();
    }
}
