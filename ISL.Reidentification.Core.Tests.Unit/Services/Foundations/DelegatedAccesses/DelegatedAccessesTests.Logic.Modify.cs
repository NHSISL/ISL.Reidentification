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
        public async Task ShouldModifyDelegatedAccessAsync()
        {
            //given
            DateTimeOffset randomDateOffset = GetRandomDateTimeOffset();

            DelegatedAccess randomDelegatedAccess = CreateRandomModifyDelegatedAccess(randomDateOffset);
            DelegatedAccess inputDelegatedAccess = randomDelegatedAccess.DeepClone();
            DelegatedAccess storageDelegatedAccess = randomDelegatedAccess.DeepClone();
            storageDelegatedAccess.UpdatedDate = storageDelegatedAccess.CreatedDate;
            DelegatedAccess updatedDelegatedAccess = inputDelegatedAccess.DeepClone();
            DelegatedAccess expectedDelegatedAccess = updatedDelegatedAccess.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateOffset);

            this.reidentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(inputDelegatedAccess.Id))
                    .ReturnsAsync(storageDelegatedAccess);

            this.reidentificationStorageBroker.Setup(broker =>
                broker.UpdateDelegatedAccessAsync(inputDelegatedAccess))
                    .ReturnsAsync(updatedDelegatedAccess);

            //when
            DelegatedAccess actualDelegatedAccess =
                await this.delegatedAccessService.ModifyDelegatedAccessAsync(inputDelegatedAccess);

            //then
            actualDelegatedAccess.Should().BeEquivalentTo(expectedDelegatedAccess);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(inputDelegatedAccess.Id),
                    Times.Once);

            this.reidentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(inputDelegatedAccess),
                    Times.Once);

            this.reidentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
