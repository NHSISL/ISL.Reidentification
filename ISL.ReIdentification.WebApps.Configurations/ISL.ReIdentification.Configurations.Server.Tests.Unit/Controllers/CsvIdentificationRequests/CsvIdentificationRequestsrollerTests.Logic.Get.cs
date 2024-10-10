﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.CsvIdentificationRequests;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RESTFulSense.Clients.Extensions;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.CsvIdentificationRequests
{
    public partial class CsvIdentificationRequestsControllerTests
    {
        [Fact]
        public async Task ShouldReturnRecordOnGetByIdsAsync()
        {
            // given
            CsvIdentificationRequest randomCsvIdentificationRequest = CreateRandomCsvIdentificationRequest();
            Guid inputId = randomCsvIdentificationRequest.Id;
            CsvIdentificationRequest storageCsvIdentificationRequest = randomCsvIdentificationRequest;
            CsvIdentificationRequest expectedCsvIdentificationRequest = storageCsvIdentificationRequest.DeepClone();

            var expectedObjectResult =
                new OkObjectResult(expectedCsvIdentificationRequest);

            var expectedActionResult =
                new ActionResult<CsvIdentificationRequest>(expectedObjectResult);

            csvIdentificationRequestServiceMock
                .Setup(service => service.RetrieveCsvIdentificationRequestByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(storageCsvIdentificationRequest);

            // when
            ActionResult<CsvIdentificationRequest> actualActionResult =
                await csvIdentificationRequestsController.GetCsvIdentificationRequestByIdAsync(inputId);

            // then
            actualActionResult.ShouldBeEquivalentTo(expectedActionResult);

            csvIdentificationRequestServiceMock
                .Verify(service => service.RetrieveCsvIdentificationRequestByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            csvIdentificationRequestServiceMock.VerifyNoOtherCalls();
        }
    }
}
