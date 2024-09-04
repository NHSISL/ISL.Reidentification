// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.AspNetCore.Mvc;

namespace ISL.Reidentification.FrontEnd.Server.Controllers
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
