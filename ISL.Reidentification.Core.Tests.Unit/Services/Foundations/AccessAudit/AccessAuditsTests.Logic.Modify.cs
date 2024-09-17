// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.ReIdentification.Core.Models.Foundations.AccessAudits;
using Moq;

namespace ISL.ReIdentification.Core.Tests.Unit.Services.Foundations.AccessAudits
{
    public partial class AccessAuditTests
    {
        [Fact]
        public async Task ShouldModifyAccessAuditAsync()
        {
            // given
            DateTimeOffset randomDateOffset = GetRandomDateTimeOffset();

            AccessAudit randomModifyAccessAudit =
                CreateRandomModifyAccessAudit(randomDateOffset);

            AccessAudit inputAccessAudit = randomModifyAccessAudit.DeepClone();
            AccessAudit storageAccessAudit = randomModifyAccessAudit.DeepClone();
            storageAccessAudit.UpdatedDate = storageAccessAudit.CreatedDate;
            AccessAudit updatedAccessAudit = inputAccessAudit.DeepClone();
            AccessAudit expectedAccessAudit = updatedAccessAudit.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.UpdateAccessAuditAsync(inputAccessAudit))
                    .ReturnsAsync(updatedAccessAudit);

            // when
            AccessAudit actualAccessAudit =
                await this.accessAuditService.ModifyAccessAuditAsync(inputAccessAudit);

            // then
            actualAccessAudit.Should().BeEquivalentTo(expectedAccessAudit);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.UpdateAccessAuditAsync(inputAccessAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
