// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.Lookups;
using ISL.ReIdentification.Core.Models.Foundations.Lookups.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RESTFulSense.Models;
using Xeptions;

namespace GitFyle.Core.Api.Tests.Unit.Services.Foundations.Lookups
{
    public partial class LookupsControllerTests
    {
        [Fact]
        public async Task PostLookupAsyncShouldReturnCreatedWhenLookupIsAdded()
        {
            // given
            Lookup lookup = CreateRandomLookup();

            mockLookupService
                .Setup(service => service.AddLookupAsync(lookup))
                .ReturnsAsync(lookup);

            // when
            var result = await lookupsController.PostLookupAsync(lookup);

            // then
            var createdResult = Assert.IsType<CreatedObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(201);
            createdResult.Value.Should().Be(lookup);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnBadRequestWhenLookupValidationExceptionOccurs()
        {
            // given
            Lookup lookup = CreateRandomLookup();

            var invalidLookupException = new InvalidLookupException(message: GetRandomString());

            var lookupValidationException = new LookupValidationException(
                message: GetRandomString(),
                innerException: invalidLookupException);

            mockLookupService
                .Setup(service => service.AddLookupAsync(lookup))
                .ThrowsAsync(lookupValidationException);

            // when
            var result = await lookupsController.PostLookupAsync(lookup);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnConflictWhenAlreadyExistsLookupExceptionOccurs()
        {
            // given
            Lookup lookup = CreateRandomLookup();
            var duplicateKeyException = new DuplicateKeyException(message: GetRandomString());

            var alreadyExistsException = new AlreadyExistsLookupException(
                message: GetRandomString(),
                innerException: duplicateKeyException,
                data: duplicateKeyException.Data);

            var dependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: alreadyExistsException);

            mockLookupService
                .Setup(service => service.AddLookupAsync(lookup))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await lookupsController.PostLookupAsync(lookup);

            // then
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            conflictResult.StatusCode.Should().Be(409);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnBadRequestWhenLookupDependencyValidationExceptionOccurs()
        {
            // given
            Lookup lookup = CreateRandomLookup();

            var lockedLookupException = new LockedLookupException(
                message: GetRandomString(),
                innerException: new Xeption());

            var dependencyValidationException = new LookupDependencyValidationException(
                message: GetRandomString(),
                innerException: lockedLookupException);

            mockLookupService
                .Setup(service => service.AddLookupAsync(lookup))
                .ThrowsAsync(dependencyValidationException);

            // when
            var result = await lookupsController.PostLookupAsync(lookup);

            // then
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnInternalServerErrorWhenLookupDependencyExceptionOccurs()
        {
            // given
            var lookup = new Lookup();

            var dbUpdateException = new DbUpdateException();

            var failedOperationLookupException =
                new FailedOperationLookupException(
                    message: "Failed operation lookup  error occurred, contact support.",
                    innerException: dbUpdateException);

            var dependencyException = new LookupDependencyException(
                message: GetRandomString(),
                innerException: failedOperationLookupException);

            mockLookupService
                .Setup(service => service.AddLookupAsync(lookup))
                .ThrowsAsync(dependencyException);

            // when
            var result = await lookupsController.PostLookupAsync(lookup);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
        }

        [Fact]
        public async Task PostLookupAsyncShouldReturnInternalServerErrorWhenLookupServiceExceptionOccurs()
        {
            // given
            var lookup = new Lookup();

            var exception = new Exception("Service error");

            var failedLookupServiceException =
                new FailedLookupServiceException(
                    message: "Failed lookup service error occurred, contact support.",
                    innerException: exception);

            var lookupServiceException = new LookupServiceException(
                message: "Service error occurred, contact support.",
                innerException: failedLookupServiceException);

            mockLookupService
                .Setup(service => service.AddLookupAsync(lookup))
                .ThrowsAsync(lookupServiceException);

            // when
            var result = await lookupsController.PostLookupAsync(lookup);

            // then
            var internalServerErrorResult = Assert.IsType<InternalServerErrorObjectResult>(result.Result);
            Assert.Equal(500, internalServerErrorResult.StatusCode);
        }
    }
}