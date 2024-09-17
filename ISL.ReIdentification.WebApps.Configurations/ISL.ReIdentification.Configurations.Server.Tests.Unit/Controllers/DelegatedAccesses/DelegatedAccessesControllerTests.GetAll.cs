// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Linq;
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
        public async Task GetAllDelegatedAccessesAsyncShouldReturnItems()
        {
            // given
            IQueryable<DelegatedAccess> randomDelegatedAccess = CreateRandomDelegatedAccesses();
            IQueryable<DelegatedAccess> storageDelegatedAccess = randomDelegatedAccess.DeepClone();
            IQueryable<DelegatedAccess> expectedDelegatedAcces = storageDelegatedAccess.DeepClone();

            mockDelegatedAccessService
                .Setup(service => service.RetrieveAllDelegatedAccessesAsync())
                .ReturnsAsync(storageDelegatedAccess);

            // when
            var result = await delegatedAccessesController.GetAsync();

            // then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedDelegatedAcces);
        }
    }
}
