// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
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
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedBadRequestObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.AddDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await this.delegatedAccessesController.PostDelegatedAccessAsync(someDelegatedAccess);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.AddDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedBadRequestObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.AddDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await this.delegatedAccessesController.PostDelegatedAccessAsync(someDelegatedAccess);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.AddDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsDelegatedAccessErrorOccurredAsync()
        {
            // given
            DelegatedAccess someDelegatedAccess = CreateRandomDelegatedAccess();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsDelegatedAccessException =
                new AlreadyExistsDelegatedAccessException(
                    message: someMessage,
                    innerException: someInnerException,
                    data: someInnerException.Data);

            var delegatedAccessDependencyValidationException =
                new DelegatedAccessDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsDelegatedAccessException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsDelegatedAccessException);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedConflictObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.AddDelegatedAccessAsync(It.IsAny<DelegatedAccess>()))
                    .ThrowsAsync(delegatedAccessDependencyValidationException);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await this.delegatedAccessesController.PostDelegatedAccessAsync(someDelegatedAccess);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.AddDelegatedAccessAsync(It.IsAny<DelegatedAccess>()),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
