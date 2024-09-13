using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using ISL.ReIdentification.Core.Services.Foundations.Lookups;

namespace ISL.ReIdentification.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookupsController : RESTFulController
    {
        private readonly ILookupService lookupService;

        public LookupsController(ILookupService lookupService) =>
            this.lookupService = lookupService;

        [HttpPost]
        public async ValueTask<ActionResult<Lookup>> PostLookupAsync(Lookup lookup)
        {
            try
            {
                Lookup addedLookup =
                    await this.lookupService.AddLookupAsync(lookup);

                return Created(addedLookup);
            }
            catch (LookupValidationException lookupValidationException)
            {
                return BadRequest(lookupValidationException.InnerException);
            }
            catch (LookupDependencyValidationException lookupValidationException)
                when (lookupValidationException.InnerException is InvalidLookupReferenceException)
            {
                return FailedDependency(lookupValidationException.InnerException);
            }
            catch (LookupDependencyValidationException lookupDependencyValidationException)
               when (lookupDependencyValidationException.InnerException is AlreadyExistsLookupException)
            {
                return Conflict(lookupDependencyValidationException.InnerException);
            }
            catch (LookupDependencyException lookupDependencyException)
            {
                return InternalServerError(lookupDependencyException);
            }
            catch (LookupServiceException lookupServiceException)
            {
                return InternalServerError(lookupServiceException);
            }
        }
    }
}