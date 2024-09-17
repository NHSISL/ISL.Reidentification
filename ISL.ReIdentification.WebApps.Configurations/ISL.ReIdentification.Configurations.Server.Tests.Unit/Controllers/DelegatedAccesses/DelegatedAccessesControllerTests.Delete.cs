// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.DelegatedAccesses
{
    public partial class DelegatedAccessesControllerTests
    {
        [Fact]
        public async Task DeleteDelegatedAccessByIdsAsyncShouldReturnDelegatedAccess()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            Guid inputId = randomDelegatedAccess.Id;
            DelegatedAccess storageDelegatedAcccess = randomDelegatedAccess;
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAcccess.DeepClone();

            mockDelegatedAccessService.Setup(service =>
                service.RemoveDelegatedAccessByIdAsync(inputId))
                .ReturnsAsync(storageDelegatedAcccess);

            // when
            var result = await this.delegatedAccessesController.DeleteDelegatedAccessByIdAsync(inputId);

            // then
            var actualResult = Assert.IsType<OkObjectResult>(result.Result);
            actualResult.StatusCode.Should().Be(200);
            actualResult.Value.Should().BeEquivalentTo(expectedDelegatedAccess);
        }
    }
}
