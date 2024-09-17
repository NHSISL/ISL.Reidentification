// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
    }
}
