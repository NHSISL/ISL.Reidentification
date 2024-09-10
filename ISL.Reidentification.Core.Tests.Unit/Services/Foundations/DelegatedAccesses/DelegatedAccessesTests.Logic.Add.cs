// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Reidentification.Core.Models.Foundations.DelegatedAccesses;
using Moq;

namespace ISL.Reidentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
{
    public partial class DelegatedAccessesTests
    {
        [Fact]
        public async Task ShouldAddDelegatedAccessAsync()
        {
            //given
            DelegatedAccess randomDelegatedAccess = CreateRandomDelegatedAccess();
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess;
            DelegatedAccess storageDelegatedAccess = inputDelegatedAccess.DeepClone();
            DelegatedAccess expectedDelegatedAccess = inputDelegatedAccess.DeepClone();

            this.reidentificationStorageBroker.Setup(broker =>
                broker.InsertDelegatedAccessAsync(inputDelegatedAccess))
                    .ReturnsAsync(storageDelegatedAccess);

            //when
            DelegatedAccess actualDelegatedAccess = await this.delegatedAccessService.AddDelegatedAccessAsync(inputDelegatedAccess);

            //then
            actualDelegatedAccess.Should().BeEquivalentTo(expectedDelegatedAccess);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.InsertDelegatedAccessAsync(inputDelegatedAccess),
                    Times.Once);

            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
