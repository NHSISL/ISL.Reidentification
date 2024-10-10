﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts;
using ISL.ReIdentification.Core.Models.Foundations.ImpersonationContexts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.ImpersonationContexts
{
    public partial class ImpersonationContextsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            ImpersonationContext someImpersonationContext = CreateRandomImpersonationContext();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<ImpersonationContext>(expectedBadRequestObjectResult);

            this.impersonationContextServiceMock.Setup(service =>
                service.AddImpersonationContextAsync(It.IsAny<ImpersonationContext>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<ImpersonationContext> actualActionResult =
                await this.impersonationContextsController.PostImpersonationContextAsync(someImpersonationContext);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.impersonationContextServiceMock.Verify(service =>
                service.AddImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Once);

            this.impersonationContextServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            ImpersonationContext someImpersonationContext = CreateRandomImpersonationContext();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<ImpersonationContext>(expectedBadRequestObjectResult);

            this.impersonationContextServiceMock.Setup(service =>
                service.AddImpersonationContextAsync(It.IsAny<ImpersonationContext>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<ImpersonationContext> actualActionResult =
                await this.impersonationContextsController.PostImpersonationContextAsync(someImpersonationContext);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.impersonationContextServiceMock.Verify(service =>
                service.AddImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Once);

            this.impersonationContextServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsImpersonationContextErrorOccurredAsync()
        {
            // given
            ImpersonationContext someImpersonationContext = CreateRandomImpersonationContext();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsImpersonationContextException =
                new AlreadyExistsImpersonationContextException(
                    message: someMessage,
                    innerException: someInnerException,
                    data: someInnerException.Data);

            var impersonationContextDependencyValidationException =
                new ImpersonationContextDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsImpersonationContextException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsImpersonationContextException);

            var expectedActionResult =
                new ActionResult<ImpersonationContext>(expectedConflictObjectResult);

            this.impersonationContextServiceMock.Setup(service =>
                service.AddImpersonationContextAsync(It.IsAny<ImpersonationContext>()))
                    .ThrowsAsync(impersonationContextDependencyValidationException);

            // when
            ActionResult<ImpersonationContext> actualActionResult =
                await this.impersonationContextsController.PostImpersonationContextAsync(someImpersonationContext);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.impersonationContextServiceMock.Verify(service =>
                service.AddImpersonationContextAsync(It.IsAny<ImpersonationContext>()),
                    Times.Once);

            this.impersonationContextServiceMock.VerifyNoOtherCalls();
        }
    }
}
