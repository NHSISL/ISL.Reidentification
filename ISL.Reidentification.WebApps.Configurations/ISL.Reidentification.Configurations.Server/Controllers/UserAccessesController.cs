// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Services.Foundations.UserAccesses;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccessesController : RESTFulController
    {
        private readonly IUserAccessService userAccessService;

        public UserAccessesController(IUserAccessService userAccessService) =>
            this.userAccessService = userAccessService;

        [HttpPost]
        public async ValueTask<ActionResult<UserAccess>> PostUserAccessAsync(UserAccess userAccess)
        {
            throw new NotImplementedException();
        }
    }
}
