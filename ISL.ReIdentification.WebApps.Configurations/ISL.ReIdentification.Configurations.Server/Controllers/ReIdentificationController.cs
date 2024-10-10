// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Orchestrations.Accesses;
using ISL.ReIdentification.Core.Services.Orchestrations.Identifications;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReIdentificationController : RESTFulController
    {
        private readonly IIdentificationCoordinationService identificationCoordinationService;

        public ReIdentificationController(IIdentificationCoordinationService identificationCoordinationService) =>
            this.identificationCoordinationService = identificationCoordinationService;

        [HttpPost]
        public async ValueTask<ActionResult<AccessRequest>>
            PostIdentificationRequestsAsync(AccessRequest accessRequest) =>
                throw new NotImplementedException();
    }
}
