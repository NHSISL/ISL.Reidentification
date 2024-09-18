﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
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
            try
            {
                DelegatedAccess addedDelegatedAccess =
                    await this.delegatedAccessService.AddDelegatedAccessAsync(delegatedAccess);

                return Created(addedDelegatedAccess);
            }
            catch (DelegatedAccessValidationException delegatedValidationException)
            {
                return BadRequest(delegatedValidationException.InnerException);
            }
            catch (DelegatedAccessDependencyValidationException delegatedAccessDependencyValidationException)
               when (delegatedAccessDependencyValidationException.InnerException
                    is AlreadyExistsDelegatedAccessException)
            {
                return Conflict(delegatedAccessDependencyValidationException.InnerException);
            }
            catch (DelegatedAccessDependencyValidationException delegatedAccessDependencyValidationException)
            {
                return BadRequest(delegatedAccessDependencyValidationException.InnerException);
            }
            catch (DelegatedAccessDependencyException delegatedAccessDependencyException)
            {
                return InternalServerError(delegatedAccessDependencyException);
            }
            catch (DelegatedAccessServiceException delegatedAccessServiceException)
            {
                return InternalServerError(delegatedAccessServiceException);
            }
        }

        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<DelegatedAccess>>> GetAsync()
        {
            try
            {
                IQueryable<DelegatedAccess> delegatedAccesses =
                    await this.delegatedAccessService.RetrieveAllDelegatedAccessesAsync();

                return Ok(delegatedAccesses);
            }
            catch (DelegatedAccessDependencyException delegatedAccessDependencyException)
            {
                return InternalServerError(delegatedAccessDependencyException);
            }
            catch (DelegatedAccessServiceException delegatedAccessServiceException)
            {
                return InternalServerError(delegatedAccessServiceException);
            }
        }

        [HttpGet("{delegatedAccessId}")]
        public async ValueTask<ActionResult<DelegatedAccess>> GetDelegatedAccessByIdAsync(Guid delegatedAccessId)
        {
            try
            {
                DelegatedAccess delegatedAccess =
                    await this.delegatedAccessService.RetrieveDelegatedAccessByIdAsync(delegatedAccessId);

                return Ok(delegatedAccess);
            }
            catch (DelegatedAccessValidationException delegatedAccessValidationException)
                when (delegatedAccessValidationException.InnerException is NotFoundDelegatedAccessException)
            {
                return NotFound(delegatedAccessValidationException.InnerException);
            }
            catch (DelegatedAccessValidationException delegatedAccessValidationException)
            {
                return BadRequest(delegatedAccessValidationException.InnerException);
            }
            catch (DelegatedAccessDependencyException delegatedAccessDependencyException)
            {
                return InternalServerError(delegatedAccessDependencyException);
            }
            catch (DelegatedAccessServiceException delegatedAccessServiceException)
            {
                return InternalServerError(delegatedAccessServiceException);
            }
        }

        [HttpDelete("{delegatedAccessId}")]
        public async ValueTask<ActionResult<DelegatedAccess>> DeleteDelegatedAccessByIdAsync(Guid delegatedAccessId)
        {
            try
            {
                DelegatedAccess deletedDelegatedAccess =
                    await this.delegatedAccessService.RemoveDelegatedAccessByIdAsync(delegatedAccessId);

                return Ok(deletedDelegatedAccess);
            }
            catch (DelegatedAccessValidationException delegatedAccessValidationException)
                when (delegatedAccessValidationException.InnerException
                    is NotFoundDelegatedAccessException)
            {
                return NotFound(delegatedAccessValidationException.InnerException);
            }
            catch (DelegatedAccessValidationException delegatedAccessValidationException)
            {
                return BadRequest(delegatedAccessValidationException.InnerException);
            }
            catch (DelegatedAccessDependencyValidationException delegatedAccessDependencyValidationException)
                when (delegatedAccessDependencyValidationException.InnerException is LockedDelegatedAccessException)
            {
                return Locked(delegatedAccessDependencyValidationException.InnerException);
            }
            catch (DelegatedAccessDependencyValidationException delegatedAccessDependencyValidationException)
            {
                return BadRequest(delegatedAccessDependencyValidationException);
            }
        }
    }
}
