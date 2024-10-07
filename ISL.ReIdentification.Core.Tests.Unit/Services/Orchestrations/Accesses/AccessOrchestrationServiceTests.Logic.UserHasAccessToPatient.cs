// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.PdsDatas;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Orchestrations.Accesses
{
    public partial class AccessOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(UserHasAccessToPatientTrue))]
        public async Task ShouldGetUserHasAccessToPatient(PdsData returnedPdsData, string userOrganisation)
        {
            // given
            string identifier = GetRandomStringWithLength(15);
            string inputIdentifier = returnedPdsData.PseudoNhsNumber;

            List<string> userOrganisations =
                new List<string> { userOrganisation };

            IQueryable<PdsData> returnedPdsDatas =
                new List<PdsData>() { returnedPdsData }
                    .AsQueryable();

            bool expectedResult = true;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(DateTimeOffset.UtcNow);

            this.pdsDataServiceMock.Setup(service =>
                service.RetrieveAllPdsDatasAsync())
                    .ReturnsAsync(returnedPdsDatas);

            // when
            bool actualResult = await this.accessOrchestrationService
                .UserHasAccessToPatientAsync(inputIdentifier, userOrganisations);

            // then
            actualResult.Should().Be(expectedResult);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.pdsDataServiceMock.Verify(service =>
                service.RetrieveAllPdsDatasAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.userAccessServiceMock.VerifyNoOtherCalls();
            this.pdsDataServiceMock.VerifyNoOtherCalls();
        }
    }
}
