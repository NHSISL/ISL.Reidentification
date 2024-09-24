// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.DelegatedAccesses
{
    public partial class DelegatedAccessesControllerTests
    {
        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnGetIfServerErrorOccurredAsync(
            Xeption serverException)
        {
            // given
            IQueryable<DelegatedAccess> someDelegatedAccesses = CreateRandomDelegatedAccesses();

            InternalServerErrorObjectResult expectedInternalServerErrorObjectResult =
                InternalServerError(serverException);

            var expectedActionResult =
                new ActionResult<IQueryable<DelegatedAccess>>(expectedInternalServerErrorObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.RetrieveAllDelegatedAccessesAsync())
                    .ThrowsAsync(serverException);

            // when
            ActionResult<IQueryable<DelegatedAccess>> actualActionResult =
                await this.delegatedAccessesController.GetAsync();

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.RetrieveAllDelegatedAccessesAsync(),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
