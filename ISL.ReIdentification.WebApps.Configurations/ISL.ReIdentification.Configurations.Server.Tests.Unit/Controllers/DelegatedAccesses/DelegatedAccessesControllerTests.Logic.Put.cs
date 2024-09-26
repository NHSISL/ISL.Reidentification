// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldReturnOkOnPutAsync()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess storageDelegatedAccess = inputDelegatedAccess.DeepClone();
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAccess.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedDelegatedAccess);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedObjectResult);

            delegatedAccessServiceMock
                .Setup(service => service.ModifyDelegatedAccessAsync(inputDelegatedAccess))
                    .ReturnsAsync(storageDelegatedAccess);

            // when
            ActionResult<DelegatedAccess> actualActionResult = await delegatedAccessesController
                .PutDelegatedAccessAsync(randomDelegatedAccess);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            delegatedAccessServiceMock
               .Verify(service => service.ModifyDelegatedAccessAsync(inputDelegatedAccess),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
