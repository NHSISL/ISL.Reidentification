// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldRetrieveDelegatedAccessByIdAsync()
        {
            // given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess storageDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess expectedDelegatedAccess = storageDelegatedAccess.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(randomDelegatedAccess.Id))
                    .ReturnsAsync(storageDelegatedAccess);

            // when
            DelegatedAccess actualDelegatedAccess =
                await delegatedAccessService.RetrieveDelegatedAccessByIdAsync(randomDelegatedAccess.Id);

            // then
            actualDelegatedAccess.Should().BeEquivalentTo(expectedDelegatedAccess);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(randomDelegatedAccess.Id),
                    Times.Once());

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
