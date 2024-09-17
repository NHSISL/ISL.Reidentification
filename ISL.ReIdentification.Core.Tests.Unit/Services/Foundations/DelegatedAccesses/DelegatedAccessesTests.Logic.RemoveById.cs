// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        public async Task ShouldRemoveDelegatedAccessByIdAsync()
        {
            // given
            Guid someDelegatedAccessId = Guid.NewGuid();
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess storageDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess inputDelegatedAccess = storageDelegatedAccess;
            DelegatedAccess removedDelegatedAccess = inputDelegatedAccess;
            DelegatedAccess expectedDelegatedAccess = removedDelegatedAccess.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccessId))
                    .ReturnsAsync(storageDelegatedAccess);

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.DeleteDelegatedAccessAsync(inputDelegatedAccess))
                    .ReturnsAsync(removedDelegatedAccess);

            // when
            DelegatedAccess actualDelegatedAccess =
                await this.delegatedAccessService.RemoveDelegatedAccessByIdAsync(someDelegatedAccessId);

            // then
            actualDelegatedAccess.Should().BeEquivalentTo(expectedDelegatedAccess);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(someDelegatedAccessId),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.DeleteDelegatedAccessAsync(storageDelegatedAccess),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
