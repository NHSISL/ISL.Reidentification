// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.DelegatedAccesses
{
    public partial class DelegatedAccessesControllerTests
    {
        [Fact]
        public async Task GetDelegatedAccessByIdsAsyncShouldReturnDelegatedAccess()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            Guid inputId = randomDelegatedAccess.Id;
            DelegatedAccess storageDelegatedAccess = randomDelegatedAccess.DeepClone();
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAccess.DeepClone();

            this.mockDelegatedAccessService
                .Setup(service => service.RetrieveDelegatedAccessByIdAsync(inputId))
                .ReturnsAsync(storageDelegatedAccess);

            // when
            var result = await delegatedAccessesController.GetDelegatedAccessByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);
        }

        [Fact]
        public async Task GetDelegatedAccessByIdsAsyncShouldReturnNotFoundWhenDelegatedAccessValidationExceptionOccurs()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputId = randomId;
            var notFoundDelegatedAccessException = new NotFoundDelegatedAccessException(message: inputId.ToString());

            var lookupValidationException = new DelegatedAccessValidationException(
                message: GetRandomString(),
                innerException: notFoundDelegatedAccessException);

            mockDelegatedAccessService
                .Setup(service => service.RetrieveDelegatedAccessByIdAsync(inputId))
                .ThrowsAsync(lookupValidationException);

            // when
            var result = await delegatedAccessesController.GetDelegatedAccessByIdAsync(inputId);

            // then
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundObjectResult.StatusCode.Should().Be(404);
        }
    }
}
