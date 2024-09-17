// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using ISL.Reidentification.Core.Models.Foundations.UserAccesses.Exceptions;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses.Exceptions;
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
            try
            {
                UserAccess addedUserAccess = await this.userAccessService.AddUserAccessAsync(userAccess);

                return Created(addedUserAccess);
            }
            catch (UserAccessValidationException userAccessValidationException)
            {
                return BadRequest(userAccessValidationException.InnerException);
            }
            catch (UserAccessDependencyValidationException userAccessDependencyValidationException)
                when (userAccessDependencyValidationException.InnerException is AlreadyExistsUserAccessException)
            {
                return Conflict(userAccessDependencyValidationException.InnerException);
            }
            catch (UserAccessDependencyValidationException userAccessDependencyValidationException)
            {
                return BadRequest(userAccessDependencyValidationException.InnerException);
            }
            catch (UserAccessDependencyException userAccessDependencyException)
            {
                return InternalServerError(userAccessDependencyException.InnerException);
            }
            catch (UserAccessServiceException userAccessServiceException)
            {
                return InternalServerError(userAccessServiceException.InnerException);
            }
        }

        [HttpGet]
        public async ValueTask<ActionResult<IQueryable<UserAccess>>> Get()
        {
            try
            {
                IQueryable<UserAccess> userAccesses = await this.userAccessService.RetrieveAllUserAccessesAsync();

                return Ok(userAccesses);
            }
            catch (UserAccessDependencyException userAccessDependencyException)
            {
                return InternalServerError(userAccessDependencyException.InnerException);
            }
            catch (UserAccessServiceException userAccessServiceException)
            {
                return InternalServerError(userAccessServiceException.InnerException);
            }
        }

        [HttpGet("{userAccessId}")]
        public async ValueTask<ActionResult<UserAccess>> GetUserAccessByIdAsync(Guid userAccessId)
        {
            try
            {
                UserAccess userAccess = await this.userAccessService.RetrieveUserAccessByIdAsync(userAccessId);

                return Ok(userAccess);
            }
            catch (UserAccessValidationException userAccessValidationException)
                when (userAccessValidationException.InnerException is NotFoundUserAccessException)
            {
                return NotFound(userAccessValidationException.InnerException);
            }
            catch (UserAccessValidationException userAccessValidationException)
            {
                return BadRequest(userAccessValidationException.InnerException);
            }
            catch (UserAccessDependencyException userAccessDependencyException)
            {
                return InternalServerError(userAccessDependencyException.InnerException);
            }
            catch (UserAccessServiceException userAccessServiceException)
            {
                return InternalServerError(userAccessServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<UserAccess>> PutUserAccessAsync(UserAccess userAccess)
        {
            try
            {
                UserAccess modifiedUserAccess = await this.userAccessService.ModifyUserAccessAsync(userAccess);

                return Ok(modifiedUserAccess);
            }
            catch (UserAccessValidationException userAccessValidationException)
            {
                return BadRequest(userAccessValidationException.InnerException);
            }
            catch (UserAccessDependencyValidationException userAccessDependencyValidationException)
                when (userAccessDependencyValidationException.InnerException is AlreadyExistsUserAccessException)
            {
                return Conflict(userAccessDependencyValidationException.InnerException);
            }
            catch (UserAccessDependencyValidationException userAccessDependencyValidationException)
            {
                return BadRequest(userAccessDependencyValidationException.InnerException);
            }
            catch (UserAccessDependencyException userAccessDependencyException)
            {
                return InternalServerError(userAccessDependencyException.InnerException);
            }
        }
    }
}
