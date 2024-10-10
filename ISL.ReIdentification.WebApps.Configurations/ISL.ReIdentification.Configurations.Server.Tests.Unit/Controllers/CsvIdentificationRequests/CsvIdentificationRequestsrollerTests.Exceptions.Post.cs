﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;
using RESTFulSense.Models;
using Xeptions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.CsvIdentificationRequests
{
    public partial class CsvIdentificationRequestsControllerTests
    {
        [Theory]
        [MemberData(nameof(ValidationExceptions))]
        public async Task ShouldReturnBadRequestOnPostIfValidationErrorOccurredAsync(Xeption validationException)
        {
            // given
            CsvIdentificationRequest someCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();

            BadRequestObjectResult expectedBadRequestObjectResult =
                BadRequest(validationException.InnerException);

            var expectedActionResult =
                new ActionResult<CsvIdentificationRequest>(expectedBadRequestObjectResult);

            this.csvIdentificationRequestServiceMock.Setup(service =>
                service.AddCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<CsvIdentificationRequest> actualActionResult =
                await this.csvIdentificationRequestsController.PostCsvIdentificationRequestAsync(
                    someCsvIdentificationRequest);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.csvIdentificationRequestServiceMock.Verify(service =>
                service.AddCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()),
                    Times.Once);

            this.csvIdentificationRequestServiceMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(ServerExceptions))]
        public async Task ShouldReturnInternalServerErrorOnPostIfServerErrorOccurredAsync(
            Xeption validationException)
        {
            // given
            CsvIdentificationRequest someCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();

            InternalServerErrorObjectResult expectedBadRequestObjectResult =
                InternalServerError(validationException);

            var expectedActionResult =
                new ActionResult<CsvIdentificationRequest>(expectedBadRequestObjectResult);

            this.csvIdentificationRequestServiceMock.Setup(service =>
                service.AddCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()))
                    .ThrowsAsync(validationException);

            // when
            ActionResult<CsvIdentificationRequest> actualActionResult =
                await this.csvIdentificationRequestsController.PostCsvIdentificationRequestAsync(someCsvIdentificationRequest);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.csvIdentificationRequestServiceMock.Verify(service =>
                service.AddCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()),
                    Times.Once);

            this.csvIdentificationRequestServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReturnConflictOnPostIfAlreadyExistsCsvIdentificationRequestErrorOccurredAsync()
        {
            // given
            CsvIdentificationRequest someCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();
            var someInnerException = new Exception();
            string someMessage = GetRandomString();

            var alreadyExistsCsvIdentificationRequestException =
                new AlreadyExistsCsvIdentificationRequestException(
                    message: someMessage,
                    innerException: someInnerException,
                    data: someInnerException.Data);

            var csvIdentificationRequestDependencyValidationException =
                new CsvIdentificationRequestDependencyValidationException(
                    message: someMessage,
                    innerException: alreadyExistsCsvIdentificationRequestException);

            ConflictObjectResult expectedConflictObjectResult =
                Conflict(alreadyExistsCsvIdentificationRequestException);

            var expectedActionResult =
                new ActionResult<CsvIdentificationRequest>(expectedConflictObjectResult);

            this.csvIdentificationRequestServiceMock.Setup(service =>
                service.AddCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()))
                    .ThrowsAsync(csvIdentificationRequestDependencyValidationException);

            // when
            ActionResult<CsvIdentificationRequest> actualActionResult =
                await this.csvIdentificationRequestsController.PostCsvIdentificationRequestAsync(someCsvIdentificationRequest);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            this.csvIdentificationRequestServiceMock.Verify(service =>
                service.AddCsvIdentificationRequestAsync(It.IsAny<CsvIdentificationRequest>()),
                    Times.Once);

            this.csvIdentificationRequestServiceMock.VerifyNoOtherCalls();
        }
    }
}
