// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.ReIdentifications;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReIdentificationController : RESTFulController
    {
        [HttpPost]
        public async ValueTask<ActionResult<List<string>>>
            PostIdentificationRequestsAsync(List<IdentificationRequest> identificationRequests) =>
                throw new NotImplementedException();
    }
}
