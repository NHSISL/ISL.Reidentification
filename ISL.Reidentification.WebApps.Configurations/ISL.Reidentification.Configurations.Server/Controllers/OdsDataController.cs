﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas.Exceptions;
using ISL.ReIdentification.Core.Services.Foundations.OdsDatas;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace ISL.ReIdentification.Configurations.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OdsDataController : RESTFulController
    {
        private readonly IOdsDataService odsDataService;

        public OdsDataController(IOdsDataService odsDataService) =>
            this.odsDataService = odsDataService;

        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<OdsData>>> GetAsync()
        {
            try
            {
                IQueryable<OdsData> odsData = await this.odsDataService.RetrieveAllOdsDatasAsync();

                return Ok(odsData);
            }
            catch (OdsDataDependencyException odsDataDependencyException)
            {
                return InternalServerError(odsDataDependencyException);
            }
            catch (OdsDataServiceException odsDataServiceException)
            {
                return InternalServerError(odsDataServiceException);
            }
        }

        [HttpGet("{odsDataId}")]
        public async ValueTask<ActionResult<OdsData>> GetOdsDataByIdAsync(Guid odsDataId)
        {
            try
            {
                OdsData odsData = await this.odsDataService.RetrieveOdsDataByIdAsync(odsDataId);

                return Ok(odsData);
            }
            catch (OdsDataValidationException odsDataValidationException)
                when (odsDataValidationException.InnerException is NotFoundOdsDataException)
            {
                return NotFound(odsDataValidationException.InnerException);
            }
            catch (OdsDataValidationException odsDataValidationException)
            {
                return BadRequest(odsDataValidationException.InnerException);
            }
            catch (OdsDataDependencyException odsDataDependencyException)
            {
                return InternalServerError(odsDataDependencyException);
            }
            catch (OdsDataServiceException odsDataServiceException)
            {
                return InternalServerError(odsDataServiceException);
            }
        }
    }
}
