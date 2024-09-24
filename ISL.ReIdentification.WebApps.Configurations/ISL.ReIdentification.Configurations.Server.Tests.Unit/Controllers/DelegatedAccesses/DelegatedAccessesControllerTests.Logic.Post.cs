// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.DelegatedAccesses
{
    public partial class DelegatedAccessesControllerTests
    {
        [Fact]
        public async Task ShouldReturnCreatedOnPostAsync()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess addedDelegatedAccess = inputDelegatedAccess.DeepClone();
            DelegatedAccess expectedDelegatedAccess = addedDelegatedAccess.DeepClone();

            var expectedObjectResult =
                new CreatedObjectResult(expectedDelegatedAccess);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedObjectResult);

            delegatedAccessServiceMock
                .Setup(service => service.AddDelegatedAccessAsync(inputDelegatedAccess))
                    .ReturnsAsync(addedDelegatedAccess);

            // when
            ActionResult<DelegatedAccess> actualActionResult = await delegatedAccessesController
                .PostDelegatedAccessAsync(randomDelegatedAccess);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            delegatedAccessServiceMock
               .Verify(service => service.AddDelegatedAccessAsync(inputDelegatedAccess),
                   Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
