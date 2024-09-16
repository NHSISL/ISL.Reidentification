// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldAddAccessAuditAsync()
        {
            // given
            AccessAudit randomAccessAudit = CreateRandomAccessAudit();
            AccessAudit inputAccessAudit = randomAccessAudit;
            AccessAudit storageAccessAudit = inputAccessAudit.DeepClone();
            AccessAudit expectedAccessAudit = inputAccessAudit.DeepClone();

            this.reIdentificationStorageBroker.Setup(broker =>
                broker.InsertAccessAuditAsync(inputAccessAudit))
                    .ReturnsAsync(storageAccessAudit);

            // when
            AccessAudit actualAccessAudit = await this.accessAuditService.AddAccessAuditAsync(inputAccessAudit);

            // then
            actualAccessAudit.Should().BeEquivalentTo(expectedAccessAudit);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.reIdentificationStorageBroker.Verify(broker =>
                broker.InsertAccessAuditAsync(inputAccessAudit),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.reIdentificationStorageBroker.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
