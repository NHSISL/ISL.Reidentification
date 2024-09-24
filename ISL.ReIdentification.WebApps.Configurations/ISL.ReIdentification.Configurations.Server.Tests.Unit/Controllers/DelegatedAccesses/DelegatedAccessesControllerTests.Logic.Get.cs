// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldReturnRecordOnGetByIdsAsync()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            Guid inputId = randomDelegatedAccess.Id;
            DelegatedAccess storageDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAccess.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedDelegatedAccess);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedObjectResult);

            delegatedAccessServiceMock
                .Setup(service => service.RetrieveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageDelegatedAccess);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await delegatedAccessesController.GetDelegatedAccessByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            delegatedAccessServiceMock
                .Verify(service => service.RetrieveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
