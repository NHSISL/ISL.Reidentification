﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeaturesController : Controller
    {
        private readonly IConfiguration configuration;

        public FeaturesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public ActionResult GetFeatures()
        {
            var activeFeatures = configuration.GetSection("Features").Get<string[]>();
            return Ok(activeFeatures);
        }
    }
}
