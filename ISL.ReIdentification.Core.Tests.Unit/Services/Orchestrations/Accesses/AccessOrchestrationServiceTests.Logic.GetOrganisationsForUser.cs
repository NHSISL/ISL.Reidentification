// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.OdsDatas;
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
            IQueryable<UserAccess> randomUserAccess = new List<UserAccess> { CreateRandomUserAccess() }.AsQueryable();
            IQueryable<UserAccess> storageUserAccess = randomUserAccess.DeepClone();
            IQueryable<OdsData> randomOdsDatas = CreateRandomOdsDatas();
            List<string> expectedOrganisations = new List<string>();

            this.reIdentificationStorageBrokerMock.Setup(broker =>
                broker.SelectAllUserAccessesAsync())
                    .ReturnsAsync(storageUserAccess);

            this.patientOrgReferenceStorageBrokerMock.Setup(broker =>
                broker.SelectAllOdsDatasAsync())
                    .ReturnsAsync(randomOdsDatas);

            // when
            List<string> actualOrganisations = await this.accessOrchestrationService
                .GetOrganisationsForUserAsync(inputUserEmail);

            // then
            actualOrganisations.Should().BeEquivalentTo(expectedOrganisations);

            this.reIdentificationStorageBrokerMock.Verify(broker =>
                broker.SelectAllUserAccessesAsync(),
                    Times.Once);

            this.patientOrgReferenceStorageBrokerMock.Verify(broker =>
                broker.SelectAllOdsDatasAsync(),
                    Times.Once);

            this.reIdentificationStorageBrokerMock.VerifyNoOtherCalls();
            this.patientOrgReferenceStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
