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
        public async Task ShouldReturnBadRequestOnDeleteIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedBadRequestObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await this.delegatedAccessesController.DeleteDelegatedAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnDeleteIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            Guid someId = Guid.NewGuid();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedBadRequestObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await this.delegatedAccessesController.DeleteDelegatedAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnNotFoundOnDeleteIfItemDoesNotExistAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            string someMessage = GetRandomString();

            var notFoundDelegatedAccessException =
                new NotFoundDelegatedAccessException(
                    message: someMessage);

            var delegatedAccessValidationException =
                new DelegatedAccessValidationException(
                    message: someMessage,
                    innerException: notFoundDelegatedAccessException);

            NotFoundObjectResult expectedNotFoundObjectResult =
                NotFound(notFoundDelegatedAccessException);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedNotFoundObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(delegatedAccessValidationException);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await this.delegatedAccessesController.DeleteDelegatedAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnLockedOnDeleteIfRecordIsLockedAsync()
        {
            // given
            Guid someId = Guid.NewGuid();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var lockedDelegatedAccessException =
                new LockedDelegatedAccessException(
                    message: someMessage,
                    innerException: someInnerException);

            var delegatedAccessDependencyValidationException =
                new DelegatedAccessDependencyValidationException(
                    message: someMessage,
                    innerException: lockedDelegatedAccessException);

            LockedObjectResult expectedConflictObjectResult =
                Locked(lockedDelegatedAccessException);

            var expectedActionResult =
                new ActionResult<DelegatedAccess>(expectedConflictObjectResult);

            this.delegatedAccessServiceMock.Setup(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(delegatedAccessDependencyValidationException);

            // when
            ActionResult<DelegatedAccess> actualActionResult =
                await this.delegatedAccessesController.DeleteDelegatedAccessByIdAsync(someId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.delegatedAccessServiceMock.Verify(service =>
                service.RemoveDelegatedAccessByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.delegatedAccessServiceMock.VerifyNoOtherCalls();
        }
    }
}
