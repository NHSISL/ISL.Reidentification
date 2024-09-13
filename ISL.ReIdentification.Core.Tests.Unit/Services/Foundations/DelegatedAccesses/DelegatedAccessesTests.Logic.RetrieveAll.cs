// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldRetrieveAllDelegatedAccessAsync()
        {
            //given
            IQueryable<DelegatedAccess> randomDelegatedAccesses = CreateRandomDelegatedAccesses();
            IQueryable<DelegatedAccess> storageDelegatedAccesses = randomDelegatedAccesses;
            IQueryable<DelegatedAccess> expectedDelegatedAccesses = storageDelegatedAccesses;

            this.ReIdentificationStorageBroker.Setup(broker =>
                broker.SelectAllDelegatedAccessesAsync())
                    .ReturnsAsync(storageDelegatedAccesses);

            //when
            IQueryable<DelegatedAccess> actualDelegatedAccesses =
                await this.delegatedAccessService.RetrieveAllDelegatedAccessesAsync();

            // then
            actualDelegatedAccesses.Should().BeEquivalentTo(expectedDelegatedAccesses);

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.SelectAllDelegatedAccessesAsync(),
                    Times.Once);

            this.ReIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}
