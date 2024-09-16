// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Services.Foundations.DelegatedAccesses;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DelegatedAccessesController : RESTFulController
    {
        private readonly IDelegatedAccessService delegatedAccessService;

        public DelegatedAccessesController(IDelegatedAccessService delegatedAccessService) =>
            this.delegatedAccessService = delegatedAccessService;

        [HttpPost]
        public async ValueTask<ActionResult<DelegatedAccess>> PostDelegatedAccessAsync(DelegatedAccess delegatedAccess)
        {
            DelegatedAccess addedDelegatedAccess = await this.delegatedAccessService.AddDelegatedAccessAsync(delegatedAccess);

            return Created(addedDelegatedAccess);
        }
    }
}
