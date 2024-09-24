// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.DelegatedAccesses
{
    public partial class DelegatedAccessesControllerTests
    {
        [Fact]
        public async Task ShouldReturnRecordsOnGetAsync()
        {
            // given
            IQueryable<DelegatedAccess> randomDelegatedAccesses = CreateRandomDelegatedAccesses();
            IQueryable<DelegatedAccess> storageDelegatedAccesses = randomDelegatedAccesses.DeepClone();
            IQueryable<DelegatedAccess> expectedDelegatedAccess = storageDelegatedAccesses.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedDelegatedAccess);

            var expectedActionResult =
                new ActionResult<IQueryable<DelegatedAccess>>(expectedObjectResult);

            delegatedAccessServiceMock
                .Setup(service => service.RetrieveAllDelegatedAccessesAsync())
                    .ReturnsAsync(storageDelegatedAccesses);

            // when
            ActionResult<IQueryable<DelegatedAccess>> actualActionResult =
                await delegatedAccessesController.GetAsync();

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            delegatedAccessServiceMock
               .Verify(service => service.RetrieveAllDelegatedAccessesAsync(),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
