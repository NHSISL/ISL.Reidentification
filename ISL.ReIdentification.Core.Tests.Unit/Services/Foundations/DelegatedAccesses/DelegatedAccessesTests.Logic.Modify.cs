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

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.SelectDelegatedAccessByIdAsync(inputDelegatedAccess.Id))
                    .ReturnsAsync(storageDelegatedAccess);

            this.reIdentificationStorageBroker.Setup(broker =>
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

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.SelectDelegatedAccessByIdAsync(inputDelegatedAccess.Id),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.UpdateDelegatedAccessAsync(inputDelegatedAccess),
                    Times.Once);

            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
