// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using ISL.ReIdentification.Core.Services.Foundations.Ods;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OdsDataController : RESTFulController
    {
        private readonly IOdsService odsService;

        public OdsDataController(IOdsService odsService) =>
            this.odsService = odsService;

        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<OdsData>>> GetAsync()
        {
            try
            {
                IQueryable<OdsData> odsData = await this.odsService.RetrieveAllOdsDatasAsync();

                return Ok(odsData);
            }
            catch (OdsDataDependencyException odsDataDependencyException)
            {
                return InternalServerError(odsDataDependencyException);
            }
        }

    }
}
