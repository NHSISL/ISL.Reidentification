// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ISL.ReIdentification.Configurations.Server.Tests.Unit.Controllers.UserAccesses
{
    public partial class UserAccessesControllerTests
    {
        [Fact]
        public async Task PutUserAccessAsyncShouldReturnOkWhenUserAccessIsUpdated()
        {
            // given
            UserAccess randomUserAccess = CreateRandomUserAccess();
            UserAccess inputUserAccess = randomUserAccess;
            UserAccess storageUserAccess = inputUserAccess.DeepClone();
            UserAccess expectedUserAccess = storageUserAccess.DeepClone();

            this.mockUserAccessService.Setup(service =>
                service.ModifyUserAccessAsync(inputUserAccess))
                    .ReturnsAsync(storageUserAccess);

            // when
            var result = await this.userAccessesController.PutUserAccessAsync(randomUserAccess);

            // then
            var createdResult = Assert.IsType<OkObjectResult>(result.Result);
            createdResult.StatusCode.Should().Be(200);
            createdResult.Value.Should().BeEquivalentTo(expectedUserAccess);
        }
    }
}
