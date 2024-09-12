// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.DelegatedAccesses;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.DelegatedAccesses
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

            this.ReIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(inputDelegatedAccess.Id))
                    .ReturnsAsync(storageDelegatedAccess);

            this.ReIdentificationStorageBroker.Setup(broker =>
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

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(inputDelegatedAccess.Id),
                    Times.Once);

            this.ReIdentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(inputDelegatedAccess),
                    Times.Once);

            this.ReIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
