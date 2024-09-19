﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using ISL.ReIdentification.Core.Services.Foundations.PdsDatas;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdsDataController : RESTFulController
    {
        private readonly IPdsDataService pdsDataService;

        public PdsDataController(IPdsDataService pdsDataService) =>
            this.pdsDataService = pdsDataService;

        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<PdsData>>> Get()
        {
            throw new NotImplementedException();
        }
    }
}
