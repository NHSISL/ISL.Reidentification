// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.UserAccesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldGetOrganisationsForUser()
        {
            // given
            string userEmail = GetRandomStringWithLength(50);
            string inputUserEmail = userEmail;
            UserAccess randomUserAccess = CreateRandomUserAccess();
            randomUserAccess.UserEmail = inputUserEmail;

            IQueryable<UserAccess> randomUserAccesses =
                new List<UserAccess> { randomUserAccess }
                    .AsQueryable();

            IQueryable<UserAccess> storageUserAccesses = randomUserAccesses.DeepClone();

            List<string> expectedOrganisations =
                new List<string>();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(DateTimeOffset.UtcNow);

            this.userAccessServiceMock.Setup(service =>
                service.RetrieveAllUserAccessesAsync())
                    .ReturnsAsync(storageUserAccesses);

            // when
            List<string> actualOrganisations = await this.accessOrchestrationService
                .GetOrganisationsForUserAsync(inputUserEmail);

            // then
            actualOrganisations.Should().BeEquivalentTo(expectedOrganisations);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.userAccessServiceMock.Verify(service =>
                service.RetrieveAllUserAccessesAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userAccessServiceMock.VerifyNoOtherCalls();
            this.pdsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
